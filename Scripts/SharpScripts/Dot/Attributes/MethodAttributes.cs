using System;

namespace Dot.Attributes
{
    public enum MethodButtonSize
    {
        Small = 0,
        Normal,
        Big,
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MethodButton : Attribute
    {
        public MethodButtonSize ButtonSize { get; set; }
        public MethodButton(MethodButtonSize size)
        {
            ButtonSize = size;
        }
    }
}
