using System;

namespace Dot.GUI.Attributes.Decorator
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class SpaceAreaAttribute : DecoratorAttribute
    {
        public float Before { get; }
        public float After { get; }

        public SpaceAreaAttribute(float before = 3.0f,float after = 3.0f,int order = 0):base(order)
        {
            Before = before;
            After = after;
        }
    }
}
