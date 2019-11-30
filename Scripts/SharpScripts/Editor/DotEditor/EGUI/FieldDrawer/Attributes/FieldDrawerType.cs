using System;

namespace DotEditor.EGUI.FieldDrawer.Attributes
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = false,Inherited = true)]
    public class FieldDrawerType : Attribute
    {
        public Type DrawerType { get; set; }
        public FieldDrawerType(Type type)
        {
            DrawerType = type;
        }
    }
}
