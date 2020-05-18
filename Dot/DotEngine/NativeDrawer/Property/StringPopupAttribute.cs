using System;

namespace Dot.NativeDrawer.Property
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class StringPopupAttribute : PropertyDrawerAttribute
    {
        public string[] Options { get; private set; }

        public StringPopupAttribute(string[] options)
        {
            Options = options;
        }
    }
}
