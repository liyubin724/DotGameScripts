using DotEditor.Core.Utilities;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DotEditor.EGUI
{
    public static class DEGUI
    {
        public static void DrawAreaLine(Rect rect, Color color)
        {
            Handles.color = color;

            var points = new Vector3[] {
                new Vector3(rect.x, rect.y, 0),
                new Vector3(rect.x + rect.width, rect.y, 0),
                new Vector3(rect.x + rect.width, rect.y + rect.height, 0),
                new Vector3(rect.x, rect.y + rect.height, 0),
            };

            var indexies = new int[] {
                0, 1, 1, 2, 2, 3, 3, 0,
            };

            Handles.DrawLines(points, indexies);
        }

        private static Stack<float> labelWidthStack = new Stack<float>();
        public static void BeginLabelWidth(float labelWidth)
        {
            labelWidthStack.Push(EditorGUIUtility.labelWidth);
            EditorGUIUtility.labelWidth = labelWidth;
        }

        public static void EndLableWidth()
        {
            if (labelWidthStack.Count > 0)
                EditorGUIUtility.labelWidth = labelWidthStack.Pop();
        }

        private static Stack<Color> guiColorStack = new Stack<Color>();
        public static void BeginGUIColor(Color color)
        {
            guiColorStack.Push(GUI.color);
            GUI.color = color;
        }
        public static void EndGUIColor()
        {
            if (guiColorStack.Count > 0)
                GUI.color = guiColorStack.Pop();
        }

        private static Stack<Color> guiBgColorStack = new Stack<Color>();
        public static void BeginGUIBackgroundColor(Color color)
        {
            guiBgColorStack.Push(GUI.backgroundColor);
            GUI.backgroundColor = color;
        }
        public static void EndGUIBackgroundColor()
        {
            if (guiBgColorStack.Count > 0)
                GUI.backgroundColor = guiBgColorStack.Pop();
        }

        private static Stack<Color> guiContentColorStack = new Stack<Color>();
        public static void BeginGUIContentColor(Color color)
        {
            guiContentColorStack.Push(GUI.contentColor);
            GUI.contentColor = color;
        }
        public static void EndGUIContentColor()
        {
            if (guiContentColorStack.Count > 0)
                GUI.contentColor = guiContentColorStack.Pop();
        }

        public static void BeginIndent()
        {
            EditorGUI.indentLevel++;
        }

        public static void EndIndent()
        {
            EditorGUI.indentLevel--;
        }

        public static string DrawAssetFolderSelection(Rect rect, string label, string assetFolder, bool isReadonly = true)
        {
            string folder = assetFolder;

            EditorGUI.BeginDisabledGroup(isReadonly);
            {
                folder = EditorGUI.TextField(new Rect(rect.x, rect.y, rect.width - 40, rect.height), label, assetFolder);
            }
            EditorGUI.EndDisabledGroup();

            if (GUI.Button(new Rect(rect.x + rect.width - 40, rect.y, 20, rect.height), new GUIContent(DEGUIResources.FolderIcon)))
            {
                string folderPath = EditorUtility.OpenFolderPanel("folder", folder, "");
                if (!string.IsNullOrEmpty(folderPath))
                {
                    folder = PathUtility.GetAssetPath(folderPath);
                }
            }
            if (GUI.Button(new Rect(rect.x + rect.width - 20, rect.y, 20, rect.height), "\u2716"))
            {
                folder = "";
            }
            return folder;
        }

        public static void DrawAssetFolderSelection(Rect rect,SerializedProperty property,bool isReadonly = true)
        {
            Rect drawRect = new Rect(rect.x, rect.y, rect.width - rect.height * 2, rect.height);
            EditorGUI.BeginDisabledGroup(isReadonly);
            {
                EditorGUI.PropertyField(drawRect,property);
            }
            EditorGUI.EndDisabledGroup();

            drawRect.x += drawRect.width;
            drawRect.width = rect.height;
            if (GUI.Button(drawRect,new GUIContent(DEGUIResources.FolderIcon)))
            {
                string folderPath = EditorUtility.OpenFolderPanel("folder", property.stringValue, "");
                if (!string.IsNullOrEmpty(folderPath))
                {
                    property.stringValue = PathUtility.GetAssetPath(folderPath);
                }
            }
            drawRect.x += drawRect.width;
            if (GUI.Button(drawRect, "\u2716"))
            {
                property.stringValue = "";
            }
        }
    }
}
