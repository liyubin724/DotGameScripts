using System;

namespace Dot.GUI.Drawer.Property
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class NewLabelAttribute : PropertyLabelAttribute
    {
        public string Label { get; private set; }
        public NewLabelAttribute(string label)
        {
            Label = label;
        }
    }
}
