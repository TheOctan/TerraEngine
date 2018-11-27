using GameEngine.Core;
using GameEngine.States.StateMachine;
using GameEngine.Util;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Gameplay.NPC;

namespace GameEngine.States
{
    public class StatePlaying : StateBase
    {
        private World world;
        private Player player;

        List<NpcSlime> slimes;

        public StatePlaying(Game game) : base(game)
        {
            world   = new World();
            player  = new Player(world);
            slimes  = new List<NpcSlime>();

            Game.Window.KeyPressed += Window_KeyPressed;
        }

        private void Window_KeyPressed(object sender, KeyEventArgs e)
        {
            if(e.Code == Keyboard.Key.Tab)
            {
                DebugRender.Enable = DebugRender.Enable ? false : true;
            }
        }

        public override void HandleInput()
        {
        }

        public override void Init()
        {
            world.GenerateWorld();
            
            player.startPosition = new Vector2f(300, 150);
            player.Spawn();

            for (int i = 0; i < 10; i++)
            {
                var s = new NpcSlime(world);
                s.startPosition = new Vector2f(Program.Rand.Next(150, 600), 150);
                s.Direction = Program.Rand.Next(0, 2) == 0 ? 1 : -1;
                s.Spawn();
                slimes.Add(s);
            }

            DebugRender.Enable = Game.VisibleCounter;
        }

        public override void Render(float alpha)
        {
            Game.Window.Draw(world);
            Game.Window.Draw(player);

            foreach (var s in slimes)
                Game.Window.Draw(s);

            DebugRender.Draw(Game.Window);
        }

        public override void Update(Time time)
        {
            player.Update();

            foreach (var s in slimes)
                s.Update();
        }
    }
}
