using System;

namespace Dot.NativeDrawer.Property
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class FilePathAttribute : PropertyDrawerAttribute
    {
        public bool IsAbsolute { get; set; } = false;

        public FilePathAttribute()
        {

        }
    }
}
