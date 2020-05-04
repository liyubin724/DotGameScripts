using System;

namespace Dot.GUI.Drawer.Property
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ReadonlyAttribute : PropertyControlAttribute
    {
    }
}
