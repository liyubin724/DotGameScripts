using DotEditor.Util;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DotEditor.Lua.Gen.Tabs
{
    public class GenGenericDrawer
    {
        private List<string> genericTypeNames;

        private ReorderableList rList;
        public GenGenericDrawer(List<string> cachedList)
        {
            genericTypeNames = cachedList;

            rList = new ReorderableList(genericTypeNames, typeof(string), false, true, true, true);
            rList.drawHeaderCallback = (rect) =>
            {
                EditorGUI.LabelField(rect, "Generic List");
            };
            rList.onAddCallback = (list) =>
           {
               list.list.Add(string.Empty);
           };
            rList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                Rect textFieldRect = rect;
                textFieldRect.width -= 70;

                genericTypeNames[index] = EditorGUI.TextField(textFieldRect, genericTypeNames[index]);

                Rect btnRect = textFieldRect;
                btnRect.x += btnRect.width+5;
                btnRect.width = 60;

                if(GUI.Button(btnRect,"Check"))
                {
                    string[] types = genericTypeNames[index].Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
                    if(types == null || types.Length<2)
                    {
                        EditorUtility.DisplayDialog("Error", "Format is Error", "OK");
                    }else
                    {
                        string genericType = types[0];
                        string[] paramTypes = new string[types.Length - 1];
                        Array.Copy(types, 1, paramTypes, 0, paramTypes.Length);

                        Type t = AssemblyUtil.GetGenericType(genericType, paramTypes);
                        if(t == null)
                        {
                            EditorUtility.DisplayDialog("Error", "Canvert is Error", "OK");
                        }else
                        {
                            EditorUtility.DisplayDialog("Success", "Success", "OK");
                        }
                    }
                }
            };
        }

        public void DoGUILayout()
        {
            rList.DoLayoutList();
        }
    }
}
