using System;

namespace DotEngine.Timeline.Item.Attr
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited =false)]
    public class ActionItemBindDataAttribute : Attribute
    {
        public Type BindDataType { get; set; }

        public ActionItemBindDataAttribute(Type bindDataType)
        {
            BindDataType = bindDataType;
        }
    }
}
