using Dot.FieldDrawer;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DotEditor.EGUI.FieldDrawer
{
    [TargetFieldType(typeof(List<string>))]
    public class StringListDrawer : AFieldDrawer
    {
        private bool isMultilineText = false;
        private int multilineHeight = 0;

        private ReorderableList rList = null;
        private List<string> valueList = null;
        public StringListDrawer(FieldInfo fieldInfo) : base(fieldInfo)
        {
            FieldMultilineText attr = fieldInfo.GetCustomAttribute<FieldMultilineText>();
            if (attr != null)
            {
                isMultilineText = true;
                multilineHeight = attr.Height;
            }
        }

        public override void SetData(object data)
        {
            base.SetData(data);
            valueList = (List<string>)fieldInfo.GetValue(data);
            if (valueList != null)
            {
                rList = new ReorderableList(valueList, typeof(float), true, true, true, true);
                rList.drawHeaderCallback = (rect) =>
                {
                    EditorGUI.LabelField(rect, fieldInfo.Name, EditorStyles.boldLabel);
                };
                rList.drawElementCallback = (rect, index, isActive, isFocused) =>
                {
                    EditorGUI.LabelField(new Rect(rect.x, rect.y, 40, rect.height), "" + index);
                    if (isMultilineText)
                    {
                        EditorGUI.TextArea(new Rect(rect.x + 40, rect.y, rect.width - 40, rect.height), valueList[index]);
                    }else
                    {
                        EditorGUI.TextField(new Rect(rect.x + 40, rect.y, rect.width - 40, rect.height), valueList[index]);
                    }
                };
                rList.onAddCallback = (list) =>
                {
                    list.list.Add("");
                };
                if(isMultilineText)
                {
                    rList.elementHeight = multilineHeight;
                }
            }
            else
            {
                rList = null;
            }
        }

        protected override void OnDraw(bool isShowDesc)
        {
            if (valueList == null)
            {
                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                {
                    EditorGUILayout.LabelField(fieldInfo.Name, "Data is null");
                    if (GUILayout.Button("New", GUILayout.Width(40)))
                    {
                        valueList = new List<string>();
                        fieldInfo.SetValue(data, valueList);

                        SetData(data);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                rList.DoLayoutList();
            }
        }
    }
}
