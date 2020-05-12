using Dot.NativeDrawer.Layout;
using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer.Layout
{
    public class BeginHorizontalDrawer : LayoutDrawer
    {
        public BeginHorizontalDrawer(LayoutAttribute attr) : base(attr)
        {
        }

        public override void OnLayoutGUI()
        {
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
        }
    }
}
