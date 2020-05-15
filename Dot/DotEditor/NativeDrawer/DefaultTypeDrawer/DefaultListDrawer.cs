using Dot.Utilities;
using DotEditor.GUIExtension;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer.DefaultTypeDrawer
{
    [CustomDefaultTypeDrawer(typeof(IList))]
    public class DefaultListDrawer : NativeTypeDrawer
    {
        private IList list = null;
        private List<NativeDrawerObject> listObjects = new List<NativeDrawerObject>();
        public DefaultListDrawer(NativeDrawerProperty property) : base(property)
        {
            InitList();
        }

        private void InitList()
        {
            list = DrawerProperty.GetValue<IList>();
            listObjects.Clear();
            Type elementType = TypeUtility.GetArrayOrListElementType(DrawerProperty.ValueType);
            if (TypeUtility.IsStructOrClass(elementType))
            {
                for (int i = 0; i < list.Count; ++i)
                {
                    listObjects.Add(new NativeDrawerObject(list[i]));
                }
            }
        }

        protected override bool IsValidProperty()
        {
            return TypeUtility.IsArrayOrList(DrawerProperty.ValueType);
        }

        protected override void OnDrawProperty(string label)
        {
            if(list == null)
            {
                list = DrawerProperty.GetValue<IList>();
            }

            EditorGUILayout.BeginVertical(EGUIStyles.BoxStyle);
            {
                EGUILayout.DrawBoxHeader(label, GUILayout.ExpandWidth(true));
                Rect lastRect = GUILayoutUtility.GetLastRect();
                Rect clearBtnRect = new Rect(lastRect.x + lastRect.width - 40, lastRect.y + 2, 40, lastRect.height - 4);
                if (GUI.Button(clearBtnRect, "C"))
                {
                    DrawerProperty.ClearArrayElement();
                    InitList();
                    EditorGUIUtility.ExitGUI();
                }

                for (int i = 0; i < list.Count; ++i)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        DoDrawElement(list[i], i);
                        if (GUILayout.Button("-", GUILayout.Width(20)))
                        {
                            DrawerProperty.RemoveArrayElementAtIndex(i);
                            InitList();
                            EditorGUIUtility.ExitGUI();
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    EGUILayout.DrawHorizontalLine();
                }
                Rect addBtnRect = GUILayoutUtility.GetRect(lastRect.width, 20);
                addBtnRect.x += addBtnRect.width - 40;
                addBtnRect.width = 40;
                if (GUI.Button(addBtnRect, "+"))
                {
                    DrawerProperty.AddArrayElement();
                    InitList();
                }
            }
            EditorGUILayout.EndVertical();
        }

        protected virtual void DoDrawElement(object value, int index)
        {
            EditorGUILayout.LabelField("" + index, "Test");
        }
    }
}
