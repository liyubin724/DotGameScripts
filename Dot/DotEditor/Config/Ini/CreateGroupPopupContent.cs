using Dot.Config.Ini;
using Dot.Core.Generic;
using DotEditor.GUIExtension;
using ReflectionMagic;
using UnityEditor;
using UnityEngine;
using PopupContent = DotEditor.GUIExtension.Windows.PopupWindowContent;
using PopWin = DotEditor.GUIExtension.Windows.PopupWindow;

namespace DotEditor.Config.Ini
{
    internal class CreateGroupPopupContent : PopupContent
    {
        private IniConfig iniConfig = null;
        private IniGroup newGroup = new IniGroup();

        protected internal override void OnGUI(Rect rect)
        {
            GUILayout.BeginArea(rect);
            {
                EditorGUILayout.LabelField(Contents.NewGroupContent, EGUIStyles.BoxedHeaderStyle);

                newGroup.Name = EditorGUILayout.TextField(Contents.GroupNameContent, newGroup.Name);
                newGroup.Comment = EditorGUILayout.TextField(Contents.GroupCommentContent, newGroup.Comment);

                string errorMsg = null;
                if (string.IsNullOrEmpty(newGroup.Name))
                {
                    errorMsg = Contents.GroupNameEmptyStr;
                }
                else
                {
                    ListDictionary<string, IniGroup> groups = iniConfig.AsDynamic().groups;
                    if(groups.ContainsKey(newGroup.Name))
                    {
                        errorMsg = Contents.GroupNameRepeatStr;
                    }
                }

                if(errorMsg != null && errorMsg.Length > 0)
                {
                    EditorGUILayout.HelpBox(errorMsg, MessageType.Error);
                }

                EditorGUI.BeginDisabledGroup(errorMsg != null && errorMsg.Length > 0);
                {
                    if (GUILayout.Button(Contents.SaveStr))
                    {
                        iniConfig.AddGroup(newGroup.Name, newGroup.Comment);
                        Window.CloseWindow();
                    }
                }
                EditorGUI.EndDisabledGroup();
            }
            GUILayout.EndArea();
        }


        internal static void ShowWin(IniConfig config,Rect rect)
        {
            var content = new CreateGroupPopupContent();
            content.iniConfig = config;

            PopWin.ShowWin(rect, content, false, true);
        }

        static class Contents
        {
            internal static GUIContent NewGroupContent = new GUIContent("Create New Group");

            internal static GUIContent GroupNameContent = new GUIContent("Name");
            internal static GUIContent GroupCommentContent = new GUIContent("Comment");

            internal static string GroupNameEmptyStr = "The name of the group is Empty";
            internal static string GroupNameRepeatStr = "The name of the group is exit in config";

            internal static string SaveStr = "Save";
        }
    }
}
