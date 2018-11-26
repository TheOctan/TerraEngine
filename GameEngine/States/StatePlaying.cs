using GameEngine.States.StateMachine;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.States
{
    public class StatePlaying : StateBase
    {
        public StatePlaying(Game game) : base(game)
        {

        }

        public override void HandleInput()
        {
            //throw new NotImplementedException();
        }

        public override void Init()
        {
            circle = new CircleShape(100.0f);
            circle.FillColor = Color.Green;
        }

        public override void Render(float alpha)
        {
            Game.Window.Draw(circle);
        }

        public override void Update(Time time)
        {
            //throw new NotImplementedException();
        }

        private CircleShape circle;
    }
}
