using System;

namespace Dot.FieldDrawer
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class FieldDesc : Attribute
    {
        public string Desc { get; set; }
        public FieldDesc(string desc)
        {
            Desc = desc;
        }
    }

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
        public int Height { get; set; }
        public FieldMultilineText(int h = 40)
        {
            Height = h;
        }
    }
}

