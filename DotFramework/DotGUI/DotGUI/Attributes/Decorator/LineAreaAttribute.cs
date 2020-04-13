using System;

namespace Dot.GUI.Attributes.Decorator
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class LineAreaAttribute : DecoratorAttribute
    {
        public float Thickness { get; }
        public float Padding { get; }
        public LineAreaAttribute(float thickness = 1.0f,float padding = 6.0f,int order = 0):base(order)
        {
            Thickness = thickness;
            Padding = padding;
        }
    }
}
