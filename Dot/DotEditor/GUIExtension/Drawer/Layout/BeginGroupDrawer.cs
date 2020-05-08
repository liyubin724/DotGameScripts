using Dot.GUI.Drawer;
using Dot.GUI.Drawer.Layout;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExtension.Drawer.Layout
{
    public class BeginGroupDrawer : LayoutDrawer
    {
        public BeginGroupDrawer(object data, FieldInfo field, DrawerAttribute attr) : base(data, field, attr)
        {
        }

        public override void DoBeginLayoutGUI()
        {
            BeginGroupAttribute attr = GetAttr<BeginGroupAttribute>();
            if (!string.IsNullOrEmpty(attr.Label))
            {
                EditorGUILayout.BeginHorizontal(Styles.headerBackgroundStyle);
                EditorGUILayout.LabelField(attr.Label, Styles.headerStyle);
                EditorGUILayout.EndHorizontal();
                UnityEngine.GUILayout.Space(-Styles.spacing * 2);
            }

            EditorGUILayout.BeginVertical(Styles.sectionBackgroundStyle);
        }

        public override void DoEndLayoutGUI()
        {
            EditorGUILayout.EndVertical();
        }

        private static class Styles
        {
            internal static readonly float spacing = 2.5f;

            internal static readonly GUIStyle headerStyle;
            internal static readonly GUIStyle headerBackgroundStyle;
            internal static readonly GUIStyle sectionBackgroundStyle;
            internal static readonly GUIStyle foldoutStyle;

            static Styles()
            {
                headerStyle = new GUIStyle(EditorStyles.boldLabel);
                headerBackgroundStyle = new GUIStyle(UnityEngine.GUI.skin.box);
                sectionBackgroundStyle = new GUIStyle(UnityEngine.GUI.skin.box)
                {
                    padding = new RectOffset(13, 12, 5, 5)
                };
                foldoutStyle = new GUIStyle(EditorStyles.foldout)
                {
                    fontStyle = FontStyle.Bold
                };
            }
        }
    }
}
