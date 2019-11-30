using System;

namespace Dot.FieldDrawer.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false,Inherited = true)]
    public class FieldReadonly : Attribute
    {
        public FieldReadonly() { }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class FieldShow : Attribute
    {
        public FieldShow() { }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class FieldHide : Attribute
    {
        public FieldHide() { }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class FieldOrder : Attribute
    {
        public int Order { get; set; }

        public FieldOrder(int order)
        {
            Order = order;
        }
    }
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class FieldMultilineText : Attribute
    {
        public FieldMultilineText()
        {

        }
    }
}

