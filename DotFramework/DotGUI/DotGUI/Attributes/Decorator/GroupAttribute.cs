using System;

namespace Dot.GUI.Attributes.Decorator
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class BeginGroupAttribute : DecoratorAttribute
    {
        public string Label { get; }

        public BeginGroupAttribute(string label = null,int order =0):base(order)
        {
            Label = label;
        }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class EndGroupAttribute : DecoratorAttribute
    {
        public EndGroupAttribute(int order = 0) : base(order)
        {
        }
    }
}
