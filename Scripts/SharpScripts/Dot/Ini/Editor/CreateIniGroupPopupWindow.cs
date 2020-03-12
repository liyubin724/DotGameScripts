using Dot.Ini;
using DotEditor.Core.EGUI;
using DotEditor.EGUI.Window;
using ReflectionMagic;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Ini
{
    internal class CreateIniGroupPopupWindow : DotPopupWindow
    {
        internal static void ShowWin(IniConfig config, Action<string> createdCallback,Rect position)
        {
            var win = ShowPopupWin<CreateIniGroupPopupWindow>(position, false);
            win.iniConfig = config;
        }

        private IniConfig iniConfig = null;
        private Action<string> onCreatedCallback = null;
        private IniGroup newGroup = new IniGroup();

        protected override void DrawElement()
        {
            EditorGUILayout.LabelField(Contents.NewGroupContent, DotEditorStyles.MiddleCenterLabel);
            newGroup.Name = EditorGUILayout.TextField(Contents.GroupNameContent, newGroup.Name);
            newGroup.Comment = EditorGUILayout.TextField(Contents.GroupCommentContent, newGroup.Comment);

            bool isNewGroupError = false;
            if (string.IsNullOrEmpty(newGroup.Name))
            {
                EditorGUILayout.HelpBox(Contents.GroupNameEmptyStr, MessageType.Error);
                isNewGroupError = true;
            }
            else
            {
                dynamic config = iniConfig.AsDynamic();
                Dictionary<string, IniGroup> groupDic = config.groupDic;
                if (groupDic.ContainsKey(newGroup.Name))
                {
                    EditorGUILayout.HelpBox(Contents.GroupNameRepeatStr, MessageType.Error);
                    isNewGroupError = true;
                }
            }

            if (!isNewGroupError)
            {
                GUILayout.FlexibleSpace();

                if (GUILayout.Button(Contents.SaveStr))
                {
                    iniConfig.AddGroup(newGroup.Name, newGroup.Comment);
                    onCreatedCallback?.Invoke(newGroup.Name);
                    Close();
                }
            }
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
