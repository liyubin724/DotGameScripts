using System;

namespace Dot.GUI.Drawer.Decorator
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class HelpAttribute : DecoratorAttribute
    {
        public string Text { get; private set; }
        public MessageType Type { get; private set; }

        public HelpAttribute(string text, MessageType type = MessageType.None)
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
