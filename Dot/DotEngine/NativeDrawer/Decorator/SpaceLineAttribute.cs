using System;

namespace Dot.NativeDrawer.Decorator
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class SpaceLineAttribute : DecoratorAttribute
    {
        public SpaceLineAttribute()
        {

        }
    }
}
