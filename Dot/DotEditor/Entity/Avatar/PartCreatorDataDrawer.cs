using DotEditor.GUIExtension;
using DotEditor.NativeDrawer;
using System;
using UnityEditor;
using UnityEngine;
using static DotEditor.Entity.Avatar.AvatarCreatorData;

namespace DotEditor.Entity.Avatar
{
    [CustomTypeDrawer(typeof(PartCreatorData))]
    public class PartCreatorDataDrawer : NativeTypeDrawer
    {
        public static Action<PartCreatorData> CreatePartBtnClick = null;
        public static Func<PartCreatorData,bool> IsPartSelected = null;
        public static Action<PartCreatorData,bool> PartSelectedChanged = null;

        private NativeDrawerObject drawerObject = null;
        public PartCreatorDataDrawer(NativeDrawerProperty property) : base(property)
        {
        }

        protected override bool IsValidProperty()
        {
            return DrawerProperty.ValueType == typeof(PartCreatorData);
        }

        protected override void OnDrawProperty(string label)
        {
            if(drawerObject == null)
            {
                drawerObject = new NativeDrawerObject(DrawerProperty.Value);
            }
            if(DrawerProperty.IsArrayElement)
            {
                PartCreatorData partCreatorData = (PartCreatorData)DrawerProperty.Value;
                bool isSelected = false;
                if(IsPartSelected!=null)
                {
                    isSelected = IsPartSelected(partCreatorData);
                }

                EditorGUILayout.BeginHorizontal();
                {
                    bool tempIsSelected = EditorGUILayout.Toggle(GUIContent.none, isSelected,GUILayout.Width(15));
                    if(tempIsSelected!=isSelected && PartSelectedChanged!=null)
                    {
                        PartSelectedChanged(partCreatorData, tempIsSelected);
                    }
                    EditorGUILayout.LabelField(label, UnityEngine.GUILayout.Width(25));
                    EditorGUILayout.BeginVertical();
                    {
                        drawerObject.OnGUILayout();
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                EGUI.BeginGUIBackgroundColor(Color.cyan);
                {
                    if(GUILayout.Button("Create Part"))
                    {
                        CreatePartBtnClick?.Invoke(partCreatorData);
                    }
                }
                EGUI.EndGUIBackgroundColor();
            }
            else
            {
                EditorGUILayout.LabelField(label);
                EditorGUI.indentLevel++;
                {
                    drawerObject.OnGUILayout();
                }
                EditorGUI.indentLevel--;
            }
        }
    }
}
