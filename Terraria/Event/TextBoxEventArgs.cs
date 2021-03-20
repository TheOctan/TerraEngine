using System;

namespace GameEngine.Event
{
	public class TextBoxEventArgs : EventArgs
	{
		public TextBoxEventArgs(string text)
		{
			Text = text;
		}

		public string Text;
	}
}
