using DotEditor.GUIExtension;
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer.DefaultTypeDrawer
{
    public abstract class NativeTypeDrawer
    {
        public object Target { get; private set; }
        public FieldInfo Field { get; private set; }
        public object Value
        {
            get
            {
                return Field.GetValue(Target);
            }
            set
            {
                Field.SetValue(Target, value);
            }
        }
        public Type ValueType
        {
            get
            {
                return Field.FieldType;
            }
        }

        public T GetValue<T>()
        {
            return (T)Value;
        }

        protected NativeTypeDrawer(object target,FieldInfo field)
        {
            Target = target;
            Field = field;
        }

        protected abstract bool IsValid();

        public void OnLayoutGUI(string label)
        {
            if(Target == null || Field == null || !IsValid())
            {
                OnInvalidDraw(label);
            }else
            {
                OnDraw(label);
            }
        }

        protected abstract void OnDraw(string label);

        protected virtual void OnInvalidDraw(string label)
        {
            EGUI.BeginGUIColor(Color.red);
            {
                EditorGUILayout.LabelField(string.IsNullOrEmpty(label) ? "" : label, "Invalid");
            }
            EGUI.EndGUIColor();
        }

    }
}
