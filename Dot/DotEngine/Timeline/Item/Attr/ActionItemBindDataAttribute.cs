using System;

namespace DotEngine.Timeline.Item.Attr
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited =false)]
    public class ActionItemBindDataAttribute : Attribute
    {
        public Type DataType { get; set; }

        public ActionItemBindDataAttribute(Type dataType)
        {
            DataType = dataType;
        }
    }
}
