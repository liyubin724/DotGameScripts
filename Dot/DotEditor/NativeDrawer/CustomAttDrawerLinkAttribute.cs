using System;

namespace DotEditor.NativeDrawer
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CustomAttDrawerLinkAttribute : Attribute
    {
        public Type AttrType { get; private set; }
        public CustomAttDrawerLinkAttribute(Type attrType)
        {
            AttrType = attrType;
        }
    }
}
