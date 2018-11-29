using GameEngine.Core;
using GameEngine.States.StateMachine;
using GameEngine.Util;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Gameplay.NPC;
using GameEngine.GUI;

namespace GameEngine.States
{
    public class StatePlaying : StateBase
    {
        private World world;
        private Player player1;
        private Player player2;
        private Label label1;
        private Label label2;

        private List<NpcSlime> slimes;

        private string[] locations = new string[1];
        private int currentLocation = 0;
        private bool isUpdate = true;

        public StatePlaying(Game game) : base(game)
        {
            world = new World();
            player1 = new Player(world)
            {
                startPosition = new Vector2f(300, 400)
            };
            player2 = new Player(world)
            {
                startPosition = new Vector2f(700, 300),
                Jump = Keyboard.Key.Up,
                Left = Keyboard.Key.Left,
                Rigt = Keyboard.Key.Right,
                BodyColor = new Color(135, 84, 56),
                HairColor = new Color(72, 193, 32),
                ShirtColor = new Color(255, 128, 0),
                LegsColor = new Color(130, 0, 130)
            };
            slimes = new List<NpcSlime>();

            label1 = new Label();
            label1.Text = "Player 0";
            label1.Origin = new Vector2f(label1.Size.X / 2f, 0);

            label2 = new Label();
            label2.Text = "Player 1";
            label2.Origin = new Vector2f(label2.Size.X / 2f, 0);

            Game.Window.KeyPressed += Window_KeyPressed;

            LoadSettingsFromFile("Levels/levelsOrder.config");
        }

        private void Window_KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Tab)
            {
                DebugRender.Enable = DebugRender.Enable ? false : true;
            }
            else if (e.Code == Keyboard.Key.F5)
            {
                LoadSettingsFromFile("Levels/levelsOrder.config");
                world.GenerateWorld(locations[currentLocation]);
            }
            else if(e.Code == Keyboard.Key.F4)
            {
                isUpdate = isUpdate ? false : true;
            }
            else if(e.Code == Keyboard.Key.Escape)
            {
                Console.WriteLine("Pause State");
                Game.Window.KeyPressed -= Window_KeyPressed;
                Game.Machine.PushState(new PauseState(game));
            }
        }

        public override void HandleInput()
        {
        }

        public override void Init()
        {
            world.GenerateWorld(locations[currentLocation]);
            
            player1.Spawn();
            player2.Spawn();

            for (int i = 0; i < 10; i++)
            {
                var s = new NpcSlime(world);
                s.startPosition = new Vector2f(Program.Rand.Next(150, 600), 400);
                s.Direction = Program.Rand.Next(0, 2) == 0 ? 1 : -1;
                s.Spawn();
                slimes.Add(s);
            }

            DebugRender.Enable = Game.VisibleCounter;
        }

        public override void Render(float alpha)
        {
            Game.Window.Draw(world);
            Game.Window.Draw(player1);
            Game.Window.Draw(player2);
            Game.Window.Draw(label1);
            Game.Window.Draw(label2);

            foreach (var s in slimes)
                Game.Window.Draw(s);

            DebugRender.Draw(Game.Window);
        }

        public override void Update(Time time)
        {
            if (isUpdate)
            {
                player1.Update();
                label1.Position = new Vector2f(player1.Position.X, player1.Position.Y - 30);

                player2.Update();
                label2.Position = new Vector2f(player2.Position.X, player2.Position.Y - 30);

                foreach (var s in slimes)
                    s.Update();
            }
        }

        private bool LoadSettingsFromFile(string fileName)
        {
            if (!File.Exists(fileName)) return false;

            locations = File.ReadAllLines(fileName);

            return true;
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
