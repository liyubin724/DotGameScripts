﻿using Dot.NativeDrawer;
using Dot.NativeDrawer.Property;
using DotEditor.GUIExtension;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer.Property
{
    public abstract class PropertyControlDrawer : AttrNativeDrawer
    {
        protected PropertyControlDrawer(PropertyControlAttribute attr) : base(attr)
        {
        }

        public abstract void OnLayoutGUIStart();
        public abstract void OnLayoutGUIEnd();
    }

    public abstract class PropertyLabelDrawer : AttrNativeDrawer
    {
        protected PropertyLabelDrawer(PropertyLabelAttribute attr) : base(attr)
        {
        }

        public abstract string GetLabel();
    }

    public abstract class PropertyDrawer : AttrNativeDrawer
    {
        public object Target { get; private set; }
        public FieldInfo Field { get; private set; }

        protected PropertyDrawer(object target,FieldInfo field , PropertyDrawerAttribute attr) : base(attr)
        {
            Target = target;
            Field = field;
        }

        public void OnLayoutGUI(string label)
        {
            if(!IsValid())
            {
                OnInvalidProperty(label);
            }else
            {
                OnProperty(label);
            }
        }

        protected abstract void OnProperty(string label);
        protected virtual void OnInvalidProperty(string label)
        {
            EGUI.BeginGUIColor(Color.red);
            {
                EditorGUILayout.LabelField(string.IsNullOrEmpty(label) ? "" : label, "Invalid");
            }
            EGUI.EndGUIColor();
        }

        protected virtual bool IsValid()
        {
            return true;
        }
    }
}
