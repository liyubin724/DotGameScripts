using UnityEditor;
using UnityEngine;

namespace DotEditor.Core.EGUI
{
    public static class DotEditorStyles
    {
        private static GUIStyle middleCenterLabel = null;
        private static GUIStyle boldLabelStyle = null;
        private static GUIStyle middleLeftLabelStyle = null;

        static DotEditorStyles()
        {
            middleCenterLabel = new GUIStyle(EditorStyles.label)
            {
                alignment = TextAnchor.MiddleCenter
            };

            boldLabelStyle = new GUIStyle(EditorStyles.label)
            {
                fontStyle = FontStyle.Bold
            };

            middleLeftLabelStyle = new GUIStyle(EditorStyles.label)
            {
                alignment = TextAnchor.MiddleLeft
            };
        }

        public static GUIStyle MiddleCenterLabel
        {
            get => middleCenterLabel;
        }

        public static GUIStyle BoldLabelStyle
        {
            get => boldLabelStyle;
        }

        
        public static GUIStyle MiddleLeftLabelStyle
        {
            get => middleLeftLabelStyle;
        }
    }
}
