using Dot.NativeDrawer.Property;
using DotEditor.GUIExtension;
using UnityEditor;

namespace DotEditor.NativeDrawer.Property
{
    [CustomAttributeDrawer(typeof(StringPopupAttribute))]
    public class StringPopupDrawer : PropertyDrawer
    {
        public StringPopupDrawer(NativeDrawerProperty drawerProperty, PropertyDrawerAttribute attr) : base(drawerProperty, attr)
        {
        }

        protected override bool IsValidProperty()
        {
            return typeof(string) == DrawerProperty.ValueType;
        }

        protected override void OnDrawProperty(string label)
        {
            var attr = GetAttr<StringPopupAttribute>();
            var value = DrawerProperty.GetValue<string>();

            label = label ?? "";

            EditorGUI.BeginChangeCheck();
            {
                value = EGUILayout.DrawPopup<string>(label, attr.Options, attr.Options, value);
            }
            if (EditorGUI.EndChangeCheck())
            {
                DrawerProperty.Value = value;
            }
        }
    }
}
