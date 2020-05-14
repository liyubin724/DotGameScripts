//using DotEditor.Core.Utilities;
//using DotEditor.GUIExtension;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Reflection;
//using UnityEditor;
//using UnityEngine;

//namespace DotEditor.NativeDrawer.DefaultTypeDrawer
//{
//    [CustomDefaultTypeDrawer(typeof(List<>))]
//    public class DefaultListDrawer : NativeTypeDrawer
//    {
//        public DefaultListDrawer(NativeDrawerProperty property) : base(property)
//        {
//        }

//        protected override bool IsValidProperty()
//        {
//            return TypeUtility.IsArrayOrList(DrawerProperty.ValueType);
//        }

//        protected override void OnDrawProperty(string label)
//        {
//            EditorGUILayout.BeginVertical(EGUIStyles.BoxStyle);
//            {
//                EGUILayout.DrawBoxHeader(label, GUILayout.ExpandWidth(true));
//                Rect lastRect = GUILayoutUtility.GetLastRect();
//                Rect clearBtnRect = new Rect(lastRect.x + lastRect.width - 40, lastRect.y + 2, 40, lastRect.height - 4);
//                if(GUI.Button(clearBtnRect,"clear"))
//                {
//                    list.Clear();
//                    EditorGUIUtility.ExitGUI();
//                }

//                for (int i = 0; i < list.Count; ++i)
//                {
//                    EditorGUILayout.BeginHorizontal();
//                    {
//                        DoDrawElement(list[i], i);
//                        if (GUILayout.Button("-", GUILayout.Width(20)))
//                        {
//                            DoRemove(i);
//                            EditorGUIUtility.ExitGUI();
//                        }
//                    }
//                    EditorGUILayout.EndHorizontal();
//                    EGUILayout.DrawHorizontalLine();
//                }
//                Rect addBtnRect = GUILayoutUtility.GetRect(lastRect.width, 20);
//                addBtnRect.x += addBtnRect.width - 40;
//                addBtnRect.width = 40;
//                if(GUI.Button(addBtnRect,"+"))
//                {
//                    DoAdd();
//                }
//            }
//            EditorGUILayout.EndVertical();
//        }

//        protected virtual void DoClear()
//        {
//            list.Clear();
//        }

//        protected virtual void DoDrawElement(object value,int index)
//        {
//            EditorGUILayout.LabelField("" + index, "Test");
//        }

//        protected virtual void DoAdd()
//        {
//            list.Add(Activator.CreateInstance(ValueType.GenericTypeArguments[0]));
//        }

//        protected virtual void DoRemove(int index)
//        {
//            list.RemoveAt(index);
//        }
//    }
//}
