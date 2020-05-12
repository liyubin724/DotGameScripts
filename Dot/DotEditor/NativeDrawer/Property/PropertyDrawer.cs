using Dot.NativeDrawer;
using Dot.NativeDrawer.Property;
using System.Reflection;
using UnityEditor;

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

        protected PropertyDrawer(object target,FieldInfo field ,NativeDrawerAttribute attr) : base(attr)
        {
            Target = target;
            Field = field;
        }

        public void OnLayoutGUI(string label)
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
