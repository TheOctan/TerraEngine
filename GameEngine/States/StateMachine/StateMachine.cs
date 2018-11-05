using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.States.StateMachine
{
    public class StateMachine
    {
        public StateMachine()
        {
            states = new Stack<StateBase>();
        }

        public void PushState(StateBase state)
        {
            if (!Empty()) states.Peek().Pause();
            states.Push(state);
            states.Peek().Init();
        }
        public void PopState()
        {
            shouldPop = true;
        }
        public void ChangeState(StateBase state)
        {
            change = state;
            shouldChange = true;
        }

        public StateBase GetCurrentState()
        {
            return states.Peek();
        }

        public bool Empty()
        {
            return states.Count == 0;
        }
        public void Reset()
        {
            shouldReset = true;
        }
        public void TryPop()
        {
            if(shouldReset)
            {
                states.Clear();
                return;
            }
            if(shouldPop && !Empty())
            {
                states.Pop();
                if (!Empty()) states.Peek().Resume();

                shouldPop = false;
            }
            if(shouldChange && !Empty())
            {
                states.Pop();
                states.Push(change);
                states.Peek().Init();

                shouldChange = false;
            }
        }

        private Stack<StateBase>    states;
        private StateBase           change;

        private bool shouldReset    = false;
        private bool shouldPop      = false;
        private bool shouldChange   = false;
    }
}
