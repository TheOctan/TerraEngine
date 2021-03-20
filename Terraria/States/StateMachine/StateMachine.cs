using System.Collections.Generic;

namespace GameEngine.States.StateMachine
{
	public class StateMachine
	{
		public bool Empty => states.Count == 0;
		public int Count => states.Count;
		public StateBase CurrentState => states.Peek();

		private Stack<StateBase> states;
		private StateBase change;

		private bool shouldReset = false;
		private bool shouldPop = false;
		private bool shouldChange = false;

		public StateMachine()
		{
			states = new Stack<StateBase>();
		}

		public void PushState(StateBase state)
		{
			if (!Empty)
			{
				states.Peek().Pause();
			}
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
		public void Reset()
		{
			shouldReset = true;
		}
		public void TryPop()
		{
			if (shouldReset)
			{
				states.Clear();
				return;
			}
			if (shouldPop && !Empty)
			{
				states.Pop();
				if (!Empty) states.Peek().Resume();

				shouldPop = false;
			}
			if (shouldChange && !Empty)
			{
				if (!Empty) states.Peek().Pause();
				states.Pop();
				states.Push(change);
				states.Peek().Init();

				shouldChange = false;
			}
		}
	}
}
