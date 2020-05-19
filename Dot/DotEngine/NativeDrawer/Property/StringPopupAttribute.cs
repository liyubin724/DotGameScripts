using System;

namespace Dot.NativeDrawer.Property
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class StringPopupAttribute : PropertyDrawerAttribute
    {
        public string[] Options { get; set; } = new string[0];

        public string MemberName { get; set; } = null;

        public StringPopupAttribute()
        {
        }
    }
}
