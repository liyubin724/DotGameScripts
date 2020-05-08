using Dot.Config.Ini;
using DotEditor.GUIExtension;
using DotEditor.GUIExtension.Windows;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DotEditor.Ini
{
    internal class CreateIniDataPopupWindow : DotPopupWindow
    {
        internal static void ShowWin(IniGroup group,Action<string,string> createdCallback,Rect position)
        {
            var win = ShowPopupWin<CreateIniDataPopupWindow>(position, false);
            win.group = group;
            win.onCreatedCallback = createdCallback;
        }

        private IniGroup group = null;
        private Action<string, string> onCreatedCallback = null;
        private IniData newData = new IniData();

        private List<string> optionValues = new List<string>();
        private ReorderableList optionValuesRList = null;
        private void OnEnable()
        {
            optionValuesRList = new ReorderableList(optionValues, typeof(string), true, true, true, true);
            optionValuesRList.drawHeaderCallback = (rect) =>
            {
                EditorGUI.LabelField(rect, Contents.OptionValuesHeadContent);
            };
            optionValuesRList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                string value = EditorGUI.TextField(rect, new GUIContent("" + index), optionValues[index]);
               if(value != optionValues[index])
                {
                    optionValues[index] = value;
                    newData.OptionValues = optionValues.ToArray();
                }
            };
            optionValuesRList.onAddCallback = (list) =>
            {
                optionValues.Add("");
                newData.OptionValues = optionValues.ToArray();
            };
        }

        protected override void DrawElement()
        {
            EditorGUILayout.LabelField(Contents.NewDataContent, DotEditorStyles.MiddleCenterLabel);
            newData.Key = EditorGUILayout.TextField(Contents.DataNameContent, newData.Key);
            newData.Comment = EditorGUILayout.TextField(Contents.DataCommentContent, newData.Comment);
            optionValuesRList.DoLayoutList();
            if (newData.OptionValues != null && newData.OptionValues.Length > 0)
            {
                newData.Value = DotEditorGUILayout.StringPopup(Contents.DataValueContent, newData.Value, newData.OptionValues);
            } else
            {
                newData.Value = EditorGUILayout.TextField(Contents.DataValueContent, newData.Value);
            }

            bool isNewDataError = false;
            if (string.IsNullOrEmpty(newData.Key))
            {
                EditorGUILayout.HelpBox(Contents.DataNameEmptyStr, MessageType.Error);
                isNewDataError = true;
            }
            else
            {
                if (group.dataDic.ContainsKey(newData.Key))
                {
                    EditorGUILayout.HelpBox(Contents.DataNameRepeatStr, MessageType.Error);
                    isNewDataError = true;
                }
            }

            if (!isNewDataError)
            {
                GUILayout.FlexibleSpace();

                if (GUILayout.Button(Contents.SaveStr))
                {
                    group.AddData(newData.Key, newData.Value, newData.Comment, newData.OptionValues);
                    onCreatedCallback?.Invoke(group.Name,newData.Key);
                    Close();
                }
            }
        }

        static class Contents
        {
            internal static GUIContent NewDataContent = new GUIContent("Create New Data");
            internal static string DataNameEmptyStr = "The name of the data is Empty";
            internal static string DataNameRepeatStr = "The name of the data is exit in config";
            internal static GUIContent DataNameContent = new GUIContent("Name");
            internal static GUIContent DataCommentContent = new GUIContent("Comment");
            internal static GUIContent DataValueContent = new GUIContent("Value");

            internal static GUIContent OptionValuesHeadContent = new GUIContent("Option Values");

            internal static string SaveStr = "Save";
        }
    }
}
