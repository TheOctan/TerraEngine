using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Event
{
    public class WidgetEvent
    {
        public WidgetEvent() : this("", 0, false)
        {
        }
        public WidgetEvent(String message, int value, bool isActive)
        {
            Message = message;
            Value = value;
            IsActive = isActive;
        }

        public String Message;
        public int Value;
        public bool IsActive;
    }
}
