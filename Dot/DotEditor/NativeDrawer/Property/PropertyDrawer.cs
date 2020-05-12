using Dot.NativeDrawer;
using System.Reflection;
using UnityEditor;

namespace DotEditor.NativeDrawer.Property
{
    public abstract class PropertyControlDrawer : AttrNativeDrawer
    {
        protected PropertyControlDrawer(NativeDrawerAttribute attr) : base(attr)
        {
        }

        public abstract void BeginDoLayoutGUI();
        public abstract void EndDoLayoutGUI();
    }

    public abstract class PropertyLabelDrawer : AttrNativeDrawer
    {
        protected PropertyLabelDrawer(NativeDrawerAttribute attr) : base(attr)
        {
        }

        public abstract string GetLabel();
    }

    public abstract class PropertyDrawer : AttrNativeDrawer
    {
        public object Target { get; private set; }
        public FieldInfo Field { get; private set; }

        protected PropertyDrawer(object target,FieldInfo field ,NativeDrawerAttribute attr) : base(attr)
        {
            Target = target;
            Field = field;
        }

        public void DoLayoutGUI(string label)
        {
            if(IsValid())
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
            EditorGUILayout.LabelField(string.IsNullOrEmpty(label) ? "" : label, "Invalid");
        }

        protected virtual bool IsValid()
        {
            return true;
        }
    }
}
