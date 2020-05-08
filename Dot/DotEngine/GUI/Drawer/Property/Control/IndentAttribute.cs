using System;

namespace Dot.GUI.Drawer.Property
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class IndentAttribute : PropertyControlAttribute
    {
        public int Indent { get; private set; }

        public IndentAttribute(int indent)
        {
            Indent = indent;
        }
    }
}
