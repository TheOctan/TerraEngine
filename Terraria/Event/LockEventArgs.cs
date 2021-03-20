using System;

namespace GameEngine.Event
{
	public class LockEventArgs : EventArgs
	{
		public LockEventArgs(bool isLocked)
		{
			IsLocked = isLocked;
		}

		public bool IsLocked;
	}
}
