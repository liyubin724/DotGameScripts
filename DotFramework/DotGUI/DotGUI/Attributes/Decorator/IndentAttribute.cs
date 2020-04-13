using System;

namespace Dot.GUI.Attributes.Decorator
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class BeginIndentAttribute : DecoratorAttribute
    {
        public int Indent { get; }
        public BeginIndentAttribute():this(1,0)
        {

        }

        public BeginIndentAttribute(int indent) : this(indent, 0)
        {
        }

        public BeginIndentAttribute(int indent,int order) : base(order)
        {
            Indent = indent;
        }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class EndIndentAttribute : DecoratorAttribute
    {
        public int Indent { get; }
        public EndIndentAttribute() : this(1, 0)
        {

        }

        public EndIndentAttribute(int indent) : this(indent, 0)
        {
        }

        public EndIndentAttribute(int indent, int order) : base(order)
        {
            Indent = indent;
        }
    }
}
