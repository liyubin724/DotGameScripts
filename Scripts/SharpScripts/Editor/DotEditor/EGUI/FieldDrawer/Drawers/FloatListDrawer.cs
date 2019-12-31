using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DotEditor.EGUI.FieldDrawer
{
    [TargetFieldType(typeof(List<float>))]
    public class FloatListDrawer : AFieldDrawer
    {
        private ReorderableList rList = null;
        private List<float> valueList = null;
        public FloatListDrawer(FieldInfo fieldInfo) : base(fieldInfo)
        {
        }

        public override void SetData(object data)
        {
            base.SetData(data);
            valueList = (List<float>)fieldInfo.GetValue(data);
            if(valueList != null)
            {
                rList = new ReorderableList(valueList, typeof(float), true, true, true, true);
                rList.drawHeaderCallback = (rect) =>
                {
                    EditorGUI.LabelField(rect, nameContent, EditorStyles.boldLabel);
                };
                rList.drawElementCallback = (rect, index, isActive, isFocused) =>
                {
                    EditorGUI.LabelField(new Rect(rect.x, rect.y, 40, rect.height), "" + index);
                    EditorGUI.FloatField(new Rect(rect.x+40,rect.y,rect.width-40,rect.height), valueList[index]);
                };
                rList.onAddCallback = (list) =>
                {
                    list.list.Add(0.0f);
                };
            }
            else
            {
                rList = null;
            }
        }

        protected override void OnDraw(bool isShowDesc)
        {
            if(valueList == null)
            {
                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                {
                    EditorGUILayout.LabelField(fieldInfo.Name,"Data is null");
                    if (GUILayout.Button("New", GUILayout.Width(40)))
                    {
                        valueList = new List<float>();
                        fieldInfo.SetValue(data, valueList);

                        SetData(data);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }else
            {
                rList.DoLayoutList();
            }
        }
    }
}
