using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Event
{
    public class WidgetEventArgs : EventArgs
    {
        public WidgetEventArgs(string message, int value, bool isActive)
        {
            Message     = message;
            Value       = value;
            IsActive    = isActive;
        }

        public string   Message;
        public int      Value;
        public bool     IsActive;
    }
}
