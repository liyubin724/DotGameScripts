﻿using UnityEditor;
using UnityEngine;

namespace DotEditor.Core.EGUI
{
    public static class DotEditorStyles
    {
        private static GUIStyle boldLabelStyle = null;
        public static GUIStyle BoldLabelStyle
        {
            get
            {
                if (boldLabelStyle == null)
                {
                    boldLabelStyle = new GUIStyle(EditorStyles.label);
                    boldLabelStyle.fontStyle = FontStyle.Bold;
                }
                return boldLabelStyle;
            }
        }
        public static GUIStyle GetBoldLabelStyle(int fontSize)
        {
            GUIStyle style = new GUIStyle(EditorStyles.label);
            style.fontStyle = FontStyle.Bold;
            style.fontSize = fontSize;

            return style;
        }

        private static GUIStyle middleLeftLabelStyle = null;
        public static GUIStyle MiddleLeftLabelStyle
        {
            get
            {
                if (middleLeftLabelStyle == null)
                {
                    middleLeftLabelStyle = new GUIStyle(EditorStyles.label);
                    middleLeftLabelStyle.alignment = TextAnchor.MiddleLeft;
                }
                return middleLeftLabelStyle;
            }
        }

        private static GUIStyle wordwrapLabelStyle = null;
        public static GUIStyle WordwrapLabelStyle
        {
            get
            {
                if(wordwrapLabelStyle == null)
                {
                    wordwrapLabelStyle = new GUIStyle(EditorStyles.label);
                    wordwrapLabelStyle.wordWrap = true;
                }
                return wordwrapLabelStyle;
            }
        }
    }
}
