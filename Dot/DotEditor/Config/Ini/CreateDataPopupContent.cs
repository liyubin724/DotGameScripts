using Dot.Config.Ini;
using Dot.Core.Generic;
using DotEditor.GUIExtension;
using ReflectionMagic;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using PopupContent = DotEditor.GUIExtension.Windows.PopupWindowContent;
using PopWin = DotEditor.GUIExtension.Windows.PopupWindow;

namespace DotEditor.Config.Ini
{
    internal class CreateDataPopupContent : PopupContent
    {
        private IniGroup group = null;
        private IniData newData = new IniData();

        private List<string> optionValues = new List<string>();
        private ReorderableList optionValuesRList = null;

        protected internal override void OnGUI(Rect rect)
        {
            if(optionValuesRList == null)
            {
                optionValuesRList = new ReorderableList(optionValues, typeof(string), true, true, true, true);
                optionValuesRList.drawHeaderCallback = (r) =>
                {
                    EditorGUI.LabelField(r, Contents.OptionValuesHeadContent);
                };
                optionValuesRList.drawElementCallback = (r, index, isActive, isFocused) =>
                {
                    string value = EditorGUI.TextField(r, new GUIContent("" + index), optionValues[index]);
                    if (value != optionValues[index])
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

            GUILayout.BeginArea(rect);
            {
                EditorGUILayout.LabelField(Contents.NewDataContent, EGUIStyles.BoxedHeaderStyle);

                newData.Key = EditorGUILayout.TextField(Contents.DataNameContent, newData.Key);
                newData.Comment = EditorGUILayout.TextField(Contents.DataCommentContent, newData.Comment);

                optionValuesRList.DoLayoutList();

                if (newData.OptionValues != null && newData.OptionValues.Length > 0)
                {
                    newData.Value = EGUILayout.StringPopup(Contents.DataValueContent, newData.Value, newData.OptionValues);
                }
                else
                {
                    newData.Value = EditorGUILayout.TextField(Contents.DataValueContent, newData.Value);
                }

                string errorMsg = null;
                if (string.IsNullOrEmpty(newData.Key))
                {
                    errorMsg = Contents.DataNameEmptyStr;
                }
                else
                {
                    ListDictionary<string, IniData> datas = group.AsDynamic().datas;
                    if(datas.ContainsKey(newData.Key))
                    {
                        errorMsg = Contents.DataNameRepeatStr;
                    }
                }
                if(errorMsg!=null && errorMsg.Length>0)
                {
                    EditorGUILayout.HelpBox(errorMsg, MessageType.Error);
                }

                EditorGUI.BeginDisabledGroup(errorMsg != null && errorMsg.Length > 0);
                {
                    if (GUILayout.Button(Contents.SaveStr))
                    {
                        group.AddData(newData.Key, newData.Value, newData.Comment, optionValues.ToArray());
                        Window.CloseWindow();
                    }
                }
                EditorGUI.EndDisabledGroup();
            }
            GUILayout.EndArea();
        }

        internal static void ShowWin(IniGroup group,Rect rect)
        {
            var content = new CreateDataPopupContent();
            content.group = group;

            PopWin.ShowWin(rect, content, false, true);
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
