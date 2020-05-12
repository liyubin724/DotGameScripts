using System;

namespace Dot.NativeDrawer.Decorator
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class HelpAttribute : DecoratorAttribute
    {
        public string Text { get; private set; }
        public HelpMessageType MessageType { get; private set; }

        public HelpAttribute(string text, HelpMessageType messageType = HelpMessageType.None)
        {
            Text = text;
            MessageType = messageType;
        }
    }

    public enum HelpMessageType
    {
        None,
        Info,
        Warning,
        Error,
    }
}
