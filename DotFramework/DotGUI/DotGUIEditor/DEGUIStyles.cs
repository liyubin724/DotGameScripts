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

        private static GUIStyle boxStyle = null;
        public static GUIStyle BoxStyle
        {
            get
            {
                if(boxStyle == null)
                {
                    boxStyle = new GUIStyle(GUI.skin.box);
                }
                return boxStyle;
            }
        }

        private static GUIStyle boxedHeaderStyle = null;
        public static GUIStyle BoxedHeaderStyle
        {
            get
            {
                if(boxedHeaderStyle == null)
                {
                    boxedHeaderStyle = new GUIStyle(GUI.skin.box)
                    {
                        fontSize = 12,
                        alignment = TextAnchor.MiddleLeft,
                        fontStyle = FontStyle.Bold,
                    };
                }
                return boxedHeaderStyle;
            }
        }

        public static GUIStyle GetTextureStyle(Texture2D texture)
        {
            GUIStyle style = new GUIStyle();
            style.normal.background = texture;
            return style;
        }
    }
}
