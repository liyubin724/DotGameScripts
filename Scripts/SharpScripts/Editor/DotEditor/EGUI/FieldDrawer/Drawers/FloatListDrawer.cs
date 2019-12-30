using Rotorz.Games.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DotEditor.EGUI.FieldDrawer
{
    [TargetFieldType(typeof(List<float>))]
    public class FloatListDrawer : AFieldDrawer
    {
        public FloatListDrawer(FieldInfo fieldInfo) : base(fieldInfo)
        {
        }

        protected override void OnDraw(object data, bool isShowDesc)
        {
            ReorderableListGUI.Title(fieldInfo.Name);
            List<float> list = (List<float>)fieldInfo.GetValue(data);
            if (list == null)
            {
                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                {
                    EditorGUILayout.LabelField("Data is null");
                    if (GUILayout.Button("New", GUILayout.Width(40)))
                    {
                        list = new List<float>();
                        fieldInfo.SetValue(data, list);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                ReorderableListGUI.ListField<float>(list, (position, value) =>
                {
                    return EditorGUI.FloatField(position, value);
                });
            }
        }
    }
}
