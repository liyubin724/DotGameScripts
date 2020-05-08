using Dot.GUI.Drawer;
using Dot.GUI.Drawer.Decorator;
using System.Reflection;
using UnityEngine;

namespace DotEditor.GUIExtension.Drawer.Decorator
{
    [CustomDrawer(typeof(BoxedHeaderAttribute))]
    public class BoxedHeaderDrawer : DecoratorDrawer
    {
        public BoxedHeaderDrawer(object data, FieldInfo field, DrawerAttribute attr) : base(data, field, attr)
        {
        }

        public override void DoLayoutGUI()
        {
            BoxedHeaderAttribute attr = GetAttr<BoxedHeaderAttribute>();
            if(attr!=null && !string.IsNullOrEmpty(attr.Header))
            {
                EGUILayout.DrawBoxHeader(attr.Header, UnityEngine.GUILayout.ExpandWidth(true));
            }
        }
    }
}
