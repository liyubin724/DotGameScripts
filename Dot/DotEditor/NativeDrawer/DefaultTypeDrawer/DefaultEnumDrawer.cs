using System;
using UnityEditor;

namespace DotEditor.NativeDrawer.DefaultTypeDrawer
{
    [CustomTypeDrawer(typeof(Enum))]
    public class DefaultEnumDrawer : NativeTypeDrawer
    {
        private bool isFlagEnum = false;
        public DefaultEnumDrawer(NativeDrawerProperty property) : base(property)
        {
            var flagAttrs = DrawerProperty.ValueType.GetCustomAttributes(typeof(FlagsAttribute),false);

            if(flagAttrs!=null && flagAttrs.Length>0)
            {
                isFlagEnum = true;
            }
        }

        protected override void OnDrawProperty(string label)
        {
            label = label ?? "";
            Enum value = DrawerProperty.GetValue<Enum>();
            EditorGUI.BeginChangeCheck();
            {
                if(isFlagEnum)
                {
                    value = EditorGUILayout.EnumFlagsField(label, value);
                }else
                {
                    value = EditorGUILayout.EnumPopup(label, value);
                }
            }
            if (EditorGUI.EndChangeCheck())
            {
                DrawerProperty.Value = value;
            }
        }

        protected override bool IsValidProperty()
        {
            return typeof(Enum).IsAssignableFrom(DrawerProperty.ValueType);
        }
    }
}
