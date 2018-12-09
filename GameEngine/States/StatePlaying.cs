using GameEngine.Core;
using GameEngine.States.StateMachine;
using GameEngine.Util;
using GameEngine.GUI;
using GameEngine.Exceptions;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Resource;
using GameEngine.Terraria.Generation;
using GameEngine.Terraria.NPC;

namespace GameEngine.States
{
    public class StatePlaying : StateBase
    {
        private bool gameOver = false;
        private bool startGame = false;
        private bool readyPlayer1 = false;
        private bool readyPlayer2 = false;
        private bool showMessage = false;

        private Text readyPlayer1Text;
        private Text readyPlayer2Text;
        private Text message;

        private RectangleShape rect;

        private Timer timer;

        private World world;

        private List<Player> players;
        private List<NpcSlime> slimes;
        private List<Label> labels;

        private string[] levelInformation = new string[1];
        private int currentLocation = 0;
        private bool isUpdate = true;

        public StatePlaying(Game game) : base(game)
        {
            readyPlayer1Text = new Text()
            {
                DisplayedString = "Player1: Press any key to start",
                Font = ResourceHolder.Fonts.Get("minecraft"),
                CharacterSize = 15,
                Position = new Vector2f(10, 10)
            };
            readyPlayer2Text = new Text()
            {
                DisplayedString = "Player2: Press any key to start",
                Font = ResourceHolder.Fonts.Get("minecraft"),
                CharacterSize = 15
            };
            readyPlayer2Text.Position = new Vector2f(Game.Window.Size.X - readyPlayer2Text.GetGlobalBounds().Width - 10, 10);

            message = new Text()
            {
                DisplayedString = "GO",
                Font = ResourceHolder.Fonts.Get("minecraft"),
                CharacterSize = 100
            };
            message.Position = new Vector2f(
                Game.Window.Size.X / 2 - message.GetGlobalBounds().Width / 2,
                Game.Window.Size.Y / 2 - message.GetLocalBounds().Height);

            rect = new RectangleShape();
            rect.FillColor = new Color(0, 0, 0, 150);
            rect.Size = new Vector2f(Game.Window.Size.X, Game.Window.Size.Y / 4f);
            rect.Position = new Vector2f(0, Game.Window.Size.Y / 2 - rect.GetGlobalBounds().Height / 1.5f);

            timer = new Timer(0, 3);
            timer.Position = new Vector2f(Game.Window.Size.X / 2f - timer.GetGlobalBounds().Width / 2f, 10);
            timer.EndTime += Timer_EndTime;

            world = new World();
            slimes = new List<NpcSlime>();
            players = new List<Player>()
            {
                new Player(world)
                {
                    ActiveColor = Color.Blue
                },
                new Player(world)
                {
                    ActiveColor = Color.Red,
                    Jump = Keyboard.Key.Up,
                    Left = Keyboard.Key.Left,
                    Rigt = Keyboard.Key.Right,
                    BodyColor = new Color(135, 84, 56),
                    HairColor = new Color(72, 193, 32),
                    ShirtColor = new Color(255, 128, 0),
                    LegsColor = new Color(130, 0, 130),
                    Scale = new Vector2f(-1, 1)
                }
            };

            labels = new List<Label>()
            {
                new Label() { Text = game.settings.NickName1 },
                new Label() { Text = game.settings.NickName2 }
            };
            foreach (var label in labels)
            {
                label.Origin = new Vector2f(label.Size.X / 2f, 0);
            }

            Game.Window.KeyPressed += Window_KeyPressed;
        }

        private void Window_KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Tab)
            {
                DebugRender.Enable = DebugRender.Enable ? false : true;
            }
            else if (e.Code == Keyboard.Key.F5)
            {
                Game.Window.KeyPressed -= Window_KeyPressed;
                Game.Machine.ChangeState(new StatePlaying(game));
            }
            else if (e.Code == Keyboard.Key.F4)
            {
                isUpdate = isUpdate ? false : true;
            }
            else if (e.Code == Keyboard.Key.Escape)
            {
                Game.Window.KeyPressed -= Window_KeyPressed;
                Game.Machine.PushState(new PauseState(game));
            }
            else if ((e.Code == Keyboard.Key.A || e.Code == Keyboard.Key.D || e.Code == Keyboard.Key.W || e.Code == Keyboard.Key.S) && !readyPlayer1)
            {
                readyPlayer1 = true;
                readyPlayer1Text.DisplayedString = "Ready";
                if (readyPlayer2) timer.Start();
            }
            else if ((e.Code == Keyboard.Key.Left || e.Code == Keyboard.Key.Right || e.Code == Keyboard.Key.Up || e.Code == Keyboard.Key.Down) && !readyPlayer2)
            {
                readyPlayer2 = true;
                readyPlayer2Text.DisplayedString = "Ready";
                if (readyPlayer1 && !startGame) timer.Start();
            }
        }

        private void Timer_EndTime(object sender, EventArgs e)
        {
            if (!startGame)
            {
                startGame = true;
                showMessage = true;
                timer.Reset(1);
                timer.Start();
            }
            else
            {
                gameOver = true;
                showMessage = true;

                foreach (var player in players)
                {
                    player.Reset();
                }

                rect.Size       = new Vector2f(Game.Window.Size.X, Game.Window.Size.Y / 2f);
                rect.Position   = new Vector2f(0, Game.Window.Size.Y - rect.GetGlobalBounds().Height);

                int countSlimes1 = slimes.Where(slime => slime.Color == players[0].ActiveColor).Count();
                int countSlimes2 = slimes.Where(slime => slime.Color == players[1].ActiveColor).Count();

                message.DisplayedString = "Game Over" +
                                          (countSlimes1 > countSlimes2 ?
                                              $"\nWins {labels[0].Text}\nScore {countSlimes1}" :
                                              countSlimes2 > countSlimes1 ? $"\nWins {labels[0].Text}\nScore {countSlimes2}" : "\nDraw");

                message.Position = new Vector2f(
                    Game.Window.Size.X / 2 - message.GetGlobalBounds().Width / 2,
                    Game.Window.Size.Y / 2);
            }
        }

        public override void HandleInput()
        {
        }

        public override void Init()
        {
            LoadSettingsFromFile("Levels/levelsOrder.config");

            try
            {
                var level = ExtractLevelInformation(currentLocation);

                world.GenerateWorld(level.levelName);

                players[0].startPosition = level.position1;
                players[1].startPosition = level.position2;

                foreach (var player in players)
                {
                    player.Spawn();
                }

                for (int i = 0; i < level.countSlimes; i++)
                {
                    var s = new NpcSlime(world);
                    s.startPosition = new Vector2f(Program.Rand.Next(50, 1200), Program.Rand.Next(25, 400));
                    s.Direction = Program.Rand.Next(0, 2) == 0 ? 1 : -1;
                    s.Spawn();
                    slimes.Add(s);
                }
            }
            catch (FormatException)
            {

            }

            DebugRender.Enable = Game.VisibleCounter;
        }

        public override void Render(float alpha)
        {
            Game.Window.Draw(world);
            foreach (var player in players)
            {
                Game.Window.Draw(player);
            }

            foreach (var label in labels)
            {
                Game.Window.Draw(label);
            }

            foreach (var s in slimes)
                Game.Window.Draw(s);

            if (readyPlayer1 && readyPlayer2)
                Game.Window.Draw(timer);

            if (!startGame)
            {
                Game.Window.Draw(readyPlayer2Text);
                Game.Window.Draw(readyPlayer1Text);
            }



            if (showMessage)
            {
                Game.Window.Draw(rect);
                Game.Window.Draw(message);
            }

            DebugRender.Draw(Game.Window);
        }

        public override void Update(Time time)
        {
            if (!isUpdate) return;

            timer.Update();

            if (timer.Second == 59) showMessage = false;

            for (int i = 0; i < players.Count; i++)
            {
                if (!gameOver && startGame) players[i].UpdateMovement();
                players[i].Update();
                labels[i].Position = new Vector2f(players[i].Position.X, players[i].Position.Y - 30);
            }

            if (startGame && !gameOver)
            {
                foreach (var player in players)
                {
                    foreach (var slime in slimes)
                    {
                        if (player.GetGlobalBounds().Intersects(slime.GetGlobalBounds()))
                        {
                            slime.Color = player.ActiveColor;
                        }
                    }
                }
            }

            foreach (var s in slimes)
                s.Update();
        }

        private bool LoadSettingsFromFile(string fileName)
        {
            if (!File.Exists(fileName)) return false;

            levelInformation = File.ReadAllLines(fileName);


            return true;
        }

        private (string levelName, Vector2f position1, Vector2f position2, int countSlimes) ExtractLevelInformation(int numberLevel)
        {
            var inf = levelInformation[numberLevel].Split(new[] { ' ' });

            if (inf.Length != 6) throw new FormatException();

            return (
                inf[0],
                new Vector2f(int.Parse(inf[1]), int.Parse(inf[2])),
                new Vector2f(int.Parse(inf[3]), int.Parse(inf[4])),
                int.Parse(inf[5])
                );
        }

        public override void Pause()
        {
            Game.Window.KeyPressed -= Window_KeyPressed;
        }

        public override void Resume()
        {
            Game.Window.KeyPressed += Window_KeyPressed;
        }
    }
}
