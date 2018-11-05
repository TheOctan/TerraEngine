using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.States.StateMachine
{
    public abstract class StateBase
    {
        public StateBase(Game game)
        {
            Game = game;
        }

        public abstract void Init();

        public abstract void HandleInput();
        public abstract void Update(Time time);
        public abstract void Render(float alpha);

        public virtual void Pause() { }
        public virtual void Resume() { }

        protected Game Game { get; set; }
    }
}
