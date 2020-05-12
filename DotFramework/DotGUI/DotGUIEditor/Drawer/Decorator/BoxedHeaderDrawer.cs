﻿using Dot.GUI.Drawer;
using Dot.GUI.Drawer.Decorator;
using System.Reflection;
using UnityEngine;

namespace DotEditor.EGUI.Drawer.Decorator
{
    [CustomDrawer(typeof(BoxedHeaderAttribute))]
    public class BoxedHeaderDrawer : DecoratorDrawer
    {
        public BoxedHeaderDrawer(object data, FieldInfo field, NativeDrawerAttribute attr) : base(data, field, attr)
        {
        }

        public override void DoLayoutGUI()
        {
            BoxedHeaderAttribute attr = GetAttr<BoxedHeaderAttribute>();
            if(attr!=null && !string.IsNullOrEmpty(attr.Header))
            {
                DEGUILayout.DrawBoxHeader(attr.Header,GUILayout.ExpandWidth(true));
            }
        }
    }
}