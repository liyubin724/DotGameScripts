﻿using System;
using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using SystemObject = System.Object;

namespace DotEditor.EGUI.FieldDrawer
{
    public abstract class AListFieldDrawer : AFieldDrawer
    {
        private ReorderableList rList = null;
        private IList valueList = null;

        protected AListFieldDrawer(FieldInfo fieldInfo) : base(fieldInfo)
        {
        }

        protected abstract Type GetDataType();
        protected abstract float GetElementHeight();
        protected abstract void DrawElement(Rect rect, IList list,int index);
        protected abstract SystemObject GetNewData();
        protected abstract IList GetNewList();

        public override void SetData(object data)
        {
            base.SetData(data);
            valueList = (IList)fieldInfo.GetValue(data);
            if (valueList != null)
            {
                rList = new ReorderableList(valueList, GetDataType(), true, true, true, true);
                rList.elementHeight = GetElementHeight();
                rList.drawHeaderCallback = (rect) =>
                {
                    EditorGUI.LabelField(rect, nameContent, EditorStyles.boldLabel);
                };
                rList.drawElementCallback = (rect, index, isActive, isFocused) =>
                {
                    EditorGUI.LabelField(new Rect(rect.x, rect.y, 30, rect.height), "" + index);
                    Rect fieldRect = new Rect(rect.x + 30, rect.y, rect.width - 30, rect.height);
                    DrawElement(fieldRect, valueList,index);
                };
                rList.onAddCallback = (list) =>
                {
                    list.list.Add(GetNewData());
                };
            }
            else
            {
                rList = null;
            }
        }

        protected override void OnDraw(bool isReadonly, bool isShowDesc)
        {
            if (valueList == null)
            {
                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                {
                    EditorGUILayout.LabelField(fieldInfo.Name, "Data is null");
                    if (GUILayout.Button("New", GUILayout.Width(40)))
                    {
                        fieldInfo.SetValue(data, GetNewList());
                        SetData(data);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                EditorGUILayout.BeginVertical();
                {
                    rList.DoLayoutList();
                }
                EditorGUILayout.EndVertical();
            }
        }

    }
}
