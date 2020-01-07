using Dot.FieldDrawer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DotEditor.EGUI.FieldDrawer
{
    [TargetFieldType(typeof(List<string>))]
    public class StringListDrawer : AListFieldDrawer
    {
        private bool isMultilineText = false;
        private int multilineHeight = 0;
        public StringListDrawer(FieldInfo fieldInfo) : base(fieldInfo)
        {
            FieldMultilineText attr = fieldInfo.GetCustomAttribute<FieldMultilineText>();
            if (attr != null)
            {
                isMultilineText = true;
                multilineHeight = attr.Height;
            }
        }

        protected override void DrawElement(Rect rect, IList list, int index)
        {
            if (isMultilineText)
            {
                list[index] = EditorGUI.TextArea(new Rect(rect.x, rect.y, rect.width , rect.height), (string)list[index]);
            }
            else
            {
                list[index] = EditorGUI.TextField(new Rect(rect.x + 40, rect.y, rect.width - 40, rect.height), (string)list[index]);
            }
        }

        protected override Type GetDataType()
        {
            return typeof(string);
        }

        protected override float GetElementHeight()
        {
            if(isMultilineText)
            {
                return multilineHeight;
            }else
            {
                return EditorGUIUtility.singleLineHeight;
            }
        }

        protected override object GetNewData()
        {
            return "";
        }

        protected override IList GetNewList()
        {
            return new List<string>();
        }
    }
}
