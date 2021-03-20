using System;

namespace GameEngine.Event
{
	public class ScrollBarEventArgs : EventArgs
	{
		public ScrollBarEventArgs(int value)
		{
			Value = value;
		}

		public int Value;
	}
}
