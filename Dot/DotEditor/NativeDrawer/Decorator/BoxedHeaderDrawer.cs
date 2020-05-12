using Dot.NativeDrawer.Decorator;
using DotEditor.GUIExtension;
using UnityEngine;

namespace DotEditor.NativeDrawer.Decorator
{
    [CustomAttDrawerLink(typeof(BoxedHeaderAttribute))]
    public class BoxedHeaderDrawer : DecoratorDrawer
    {
        public BoxedHeaderDrawer(DecoratorAttribute attr) : base(attr)
        {
        }

        public override void OnLayoutGUI()
        {
            BoxedHeaderAttribute attr = GetAttr<BoxedHeaderAttribute>();
            EGUILayout.DrawBoxHeader(attr.Header, GUILayout.ExpandWidth(true));
        }
    }
}
