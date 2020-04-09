using DotEditor.Core.Utilities;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEditor.EGUI
{
    public static class DEGUI
    {
        #region Draw lines
        /// <summary>
        /// 在指定的区域内绘制水平线
        /// </summary>
        /// <param name="rect"></param>
        public static void DrawHorizontalLine(Rect rect)
        {
            DrawHorizontalLine(rect, DEGUIResources.gray);
        }

        /// <summary>
        /// 在指定的区域内绘制水平线
        /// </summary>
        /// <param name="rect">绘制区域</param>
        /// <param name="thickness">线宽</param>
        /// <param name="padding">与上方的间距</param>
        /// <param name="color">绘制使用的颜色</param>
        public static void DrawHorizontalLine(Rect rect , Color color , float thickness = 0.75f,float padding = 6.0f)
        {
            rect.y += padding * 0.5f;
            rect.height = thickness;
            EditorGUI.DrawRect(rect, color);
        }

        /// <summary>
        /// 在指定区域内绘制垂直水平线
        /// </summary>
        /// <param name="rect"></param>
        public static void DrawVerticalLine(Rect rect)
        {
            DrawVerticalLine(rect, DEGUIResources.gray);
        }

        public static void DrawVerticalLine(Rect rect, Color color, float thickness = 0.75f, float padding = 6.0f )
        {
            rect.x += padding * 0.5f;
            rect.width = thickness;
            EditorGUI.DrawRect(rect, color);
        }

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

        #endregion

        /// <summary>
        /// 绘制对象的预览图
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="uObj"></param>
        public static void DrawAssetPreview(Rect rect,UnityObject uObj)
        {
            var previewTexture = AssetPreview.GetAssetPreview(uObj);
            if(previewTexture!=null)
            {
                EditorGUI.LabelField(rect, GUIContent.none, DEGUIStyles.GetTextureStyle(previewTexture));
            }
        }

        public static void DrawBox(Rect rect)
        {
            GUIStyle boxStyle = DEGUIStyles.BoxStyle;
            boxStyle.Draw(rect, false, false, false, false);
        }

        public static void DrawHeader(Rect rect,string label)
        {
            EditorGUI.LabelField(rect, label, DEGUIStyles.HeaderStyle);
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
