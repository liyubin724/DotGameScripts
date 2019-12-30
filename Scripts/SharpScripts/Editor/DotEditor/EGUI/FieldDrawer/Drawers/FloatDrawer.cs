﻿using System.Reflection;
using UnityEditor;

namespace DotEditor.EGUI.FieldDrawer
{
    [TargetFieldType(typeof(float))]
    public class FloatDrawer : AFieldDrawer
    {
        public FloatDrawer(FieldInfo fieldInfo) : base(fieldInfo)
        {
        }

        protected override void OnDraw(object data, bool isShowDesc)
        {
            float value = (float)fieldInfo.GetValue(data);

            EditorGUI.BeginChangeCheck();
            {
                value = EditorGUILayout.FloatField(fieldInfo.Name, value);
            }
            if (EditorGUI.EndChangeCheck())
            {
                fieldInfo.SetValue(data, value);
            }
        }
    }
}
