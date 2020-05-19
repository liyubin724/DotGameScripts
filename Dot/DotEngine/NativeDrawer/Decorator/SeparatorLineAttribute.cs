using System;

namespace Dot.NativeDrawer.Decorator
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class SeparatorLineAttribute : DecoratorAttribute
    {
        public SeparatorLineAttribute()
        {
        }
    }
}
