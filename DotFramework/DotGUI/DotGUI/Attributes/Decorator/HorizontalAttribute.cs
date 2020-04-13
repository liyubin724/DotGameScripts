using System;

namespace Dot.GUI.Attributes.Decorator
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class BeginHorizontalAttribute : DecoratorAttribute
    {
        public BeginHorizontalAttribute(int order= 0):base(order)
        {
        }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class EndHorizontalAttribute : DecoratorAttribute
    {
        public EndHorizontalAttribute(int order = 0) : base(order)
        {

        }
    }
}
