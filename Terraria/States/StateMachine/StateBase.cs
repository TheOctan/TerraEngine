using GameEngine.Core;
using SFML.System;

namespace GameEngine.States.StateMachine
{
    public abstract class StateBase
    {
        public StateBase(Game game)
        {
            this.game = game;
        }

        public abstract void Init();

        public virtual void HandleInput()
		{}
        public abstract void Update(Time time);
        public abstract void Render(float alpha);

        public virtual void Pause() { }
        public virtual void Resume() { }

        protected Game game { get; set; }
    }
}
