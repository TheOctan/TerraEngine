using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Event
{
    public class WidgetEventArgs : EventArgs
    {
        public WidgetEventArgs(WidgetEvent e)
        {
            Message = e.Message;
            Value = e.Value;
            IsActive = e.IsActive;
        }

        public String   Message;
        public int      Value;
        public bool     IsActive;
    }
}
