using System;

namespace Dot.GUI.Attributes.Decorator
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class HeaderAreaAttribute : DecoratorAttribute
    {
        public string Label { get; }
        public HeaderAreaAttribute(string label,int order = 0):base(order)
        {
            Label = label;
        }
    }
}
