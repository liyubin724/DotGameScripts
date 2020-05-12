﻿using Dot.NativeDrawer;
using Dot.NativeDrawer.Layout;
using DotEditor.GUIExtension;
using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer.Layout
{
    [CustomAttDrawerLink(typeof(BeginGroupAttribute))]
    public class BeginGroupDrawer : LayoutDrawer
    {
        public BeginGroupDrawer(NativeDrawerAttribute attr) : base(attr)
        {
        }

        public override void OnLayoutGUI()
        {
            BeginGroupAttribute attr = GetAttr<BeginGroupAttribute>();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
            if(!string.IsNullOrEmpty(attr.Label))
            {
                EGUILayout.DrawBoxHeader(attr.Label, GUILayout.ExpandWidth(true));
            }
        }
    }
}