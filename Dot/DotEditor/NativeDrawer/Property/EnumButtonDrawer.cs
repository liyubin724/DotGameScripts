using Dot.NativeDrawer.Property;
using System;
using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer.Property
{
    [CustomAttributeDrawer(typeof(EnumButtonAttribute))]
    public class EnumButtonDrawer : PropertyDrawer
    {
        public EnumButtonDrawer(NativeDrawerProperty drawerProperty, PropertyDrawerAttribute attr) : base(drawerProperty, attr)
        {
            
        }

        protected override bool IsValidProperty()
        {
            return typeof(Enum).IsAssignableFrom(DrawerProperty.ValueType);
        }

        protected override void OnDrawProperty(string label)
        {
            var flagAttrs = DrawerProperty.ValueType.GetCustomAttributes(typeof(FlagsAttribute), false);
            bool isFlagEnum = false;
            if (flagAttrs != null && flagAttrs.Length > 0)
            {
                isFlagEnum = true;
            }
            string[] enumNames = Enum.GetNames(DrawerProperty.ValueType);

            label = label ?? "";
            int value = DrawerProperty.GetValue<int>();
            EditorGUI.BeginChangeCheck();
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField(label,GUILayout.MaxWidth(120));
                    for (int i = 0; i < enumNames.Length; ++i)
                    {
                        int tValue = (int)Enum.Parse(DrawerProperty.ValueType, enumNames[i]);

                        bool isSelected = false;
                        if (isFlagEnum)
                        {
                            isSelected = (value & tValue) > 0;
                        }
                        else
                        {
                            isSelected = tValue == value;
                        }

                        bool newIsSelected = GUILayout.Toggle(isSelected, enumNames[i], EditorStyles.toolbarButton);
                        if(newIsSelected!=isSelected)
                        {
                            if (newIsSelected)
                            {
                                if(isFlagEnum)
                                {
                                    value |= tValue;
                                }else
                                {
                                    value = tValue;
                                }
                            }else
                            {
                                if (isFlagEnum)
                                {
                                    value &= ~tValue;
                                }
                            }
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            if (EditorGUI.EndChangeCheck())
            {
                DrawerProperty.Value = Enum.ToObject(DrawerProperty.ValueType,value);
            }
        }
    }
}
