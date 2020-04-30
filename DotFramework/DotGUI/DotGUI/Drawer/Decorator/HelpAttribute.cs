using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.GUI.Drawer.Decorator
{
    public class HelpAttribute : DecoratorDrawerAttribute
    {
        public string Text { get; private set; }
        public MessageType Type { get; private set; }

        public HelpAttribute(string text,MessageType type = MessageType.None)
        {
            Text = text;
            Type = type;
        }
    }

    public enum MessageType
    {
        None,
        Info,
        Warning,
        Error,
    }
}
