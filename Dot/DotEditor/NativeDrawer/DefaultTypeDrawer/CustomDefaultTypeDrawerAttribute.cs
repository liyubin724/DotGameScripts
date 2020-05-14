using System;

namespace DotEditor.NativeDrawer.DefaultTypeDrawer
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited =false)]
    public class CustomDefaultTypeDrawerAttribute : Attribute
    {
        public Type Target { get; private set; }
        public CustomDefaultTypeDrawerAttribute(Type target)
        {
            Target = target;
        }
    }
}
