﻿using Dot.NativeDrawer;
using Dot.NativeDrawer.Property;
using UnityEditor;

namespace DotEditor.NativeDrawer.Property
{
    [CustomAttDrawerLink(typeof(IndentAttribute))]
    public class IndentDrawer : PropertyControlDrawer
    {
        public IndentDrawer(NativeDrawerAttribute attr) : base(attr)
        {
        }

        public override void OnLayoutGUIStart()
        {
            IndentAttribute attr = GetAttr<IndentAttribute>();
            EditorGUI.indentLevel += attr.Indent;
        }

        public override void OnLayoutGUIEnd()
        {
            IndentAttribute attr = GetAttr<IndentAttribute>();
            EditorGUI.indentLevel -= attr.Indent;
        }
    }
}
