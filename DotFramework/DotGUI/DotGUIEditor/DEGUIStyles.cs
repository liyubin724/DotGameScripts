using UnityEditor;
using UnityEngine;

namespace DotEditor.EGUI
{
    public static class DEGUIStyles
    {
        private static GUIStyle middleCenterLabel = null;
        public static GUIStyle MiddleCenterLabel
        {
            get
            {
                if (middleCenterLabel == null)
                {
                    middleCenterLabel = new GUIStyle(EditorStyles.label)
                    {
                        alignment = TextAnchor.MiddleCenter
                    };
                }
                return middleCenterLabel;
            }
        }

        private static GUIStyle boldLabelStyle = null;
        public static GUIStyle BoldLabelStyle
        {
            get
            {
                if (boldLabelStyle == null)
                {
                    boldLabelStyle = new GUIStyle(EditorStyles.label)
                    {
                        fontStyle = FontStyle.Bold,
                        fixedHeight = 20,
                    };
                }
                return boldLabelStyle;
            }
        }
        private static GUIStyle middleLeftLabelStyle = null;
        public static GUIStyle MiddleLeftLabelStyle
        {
            get
            {
                if (middleLeftLabelStyle == null)
                {
                    middleLeftLabelStyle = new GUIStyle(EditorStyles.label)
                    {
                        alignment = TextAnchor.MiddleLeft
                    };
                }
                return middleLeftLabelStyle;
            }
        }
    }
}
