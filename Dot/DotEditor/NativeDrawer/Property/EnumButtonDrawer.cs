using Dot.NativeDrawer.Property;
using DotEditor.GUIExtension;
using System;
using System.Collections.Generic;
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

            label = label ?? "";
            int value = Convert.ToInt32(DrawerProperty.Value);
            EditorGUI.BeginChangeCheck();
            {
                if(isFlagEnum)
                {
                    value = DrawFlagEnum(label,value);
                }else
                {
                    value = DrawEnum(label,value);
                }
            }
            if (EditorGUI.EndChangeCheck())
            {
                DrawerProperty.Value = Enum.ToObject(DrawerProperty.ValueType, value);
            }
        }

        private int DrawEnum(string label, int value)
        {
            string[] enumNames = Enum.GetNames(DrawerProperty.ValueType);
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField(label, GUILayout.MaxWidth(220));

                for (int i = 0; i < enumNames.Length; ++i)
                {
                    int tValue = Convert.ToInt32(Enum.Parse(DrawerProperty.ValueType, enumNames[i]));

                    bool isSelected = tValue == value;

                    bool newIsSelected = GUILayout.Toggle(isSelected, enumNames[i], EditorStyles.toolbarButton, GetLayoutOptions());
                    if (newIsSelected != isSelected && newIsSelected)
                    {
                        value = tValue;
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            return value;
        }

        private int DrawFlagEnum(string label, int value)
        {
            string[] enumNames = Enum.GetNames(DrawerProperty.ValueType);

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField(label, GUILayout.MaxWidth(220));
                GUILayout.FlexibleSpace();
                if(GUILayout.Button("Everything", EditorStyles.toolbarButton ,GUILayout.Width(120)))
                {
                    value = 0;
                    for (int i = 0; i < enumNames.Length; ++i)
                    {
                        int tValue = Convert.ToInt32(Enum.Parse(DrawerProperty.ValueType, enumNames[i]));
                        value |= tValue;
                    }
                }
                if(GUILayout.Button("Nothing", EditorStyles.toolbarButton, GUILayout.Width(120)))
                {
                    value = 0;
                }
            }
            EditorGUILayout.EndHorizontal();
            EGUI.BeginIndent();
            {
                EditorGUILayout.BeginHorizontal();
                {
                    for (int i = 0; i < enumNames.Length; ++i)
                    {
                        int tValue = Convert.ToInt32(Enum.Parse(DrawerProperty.ValueType, enumNames[i]));

                        bool isSelected = (value & tValue) > 0;
                        bool newIsSelected = GUILayout.Toggle(isSelected, enumNames[i], EditorStyles.toolbarButton, GetLayoutOptions());
                        if (newIsSelected != isSelected)
                        {
                            if (newIsSelected)
                            {
                                value |= tValue;
                            }
                            else
                            {
                                value &= ~tValue;
                            }
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EGUI.EndIndent();

            return value;
        }

        private GUILayoutOption[] options = null;
        private GUILayoutOption[] GetLayoutOptions()
        {
            if(options == null)
            {
                var attr = GetAttr<EnumButtonAttribute>();

                List<GUILayoutOption> oList = new List<GUILayoutOption>();
                if(attr.MaxWidth>0)
                {
                    oList.Add(GUILayout.MaxWidth(attr.MaxWidth));
                }
                if(attr.MinWidth>0)
                {
                    oList.Add(GUILayout.MinWidth(attr.MinWidth));
                }

                options = oList.ToArray();
            }

            return options;
        }
    }
}
