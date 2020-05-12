using Dot.NativeDrawer;
using Dot.NativeDrawer.Layout;
using UnityEditor;

namespace DotEditor.NativeDrawer.Layout
{
    [CustomAttDrawerLink(typeof(EndHorizontalAttribute))]
    public class EndHorizontalDrawer : LayoutDrawer
    {
        public EndHorizontalDrawer(NativeDrawerAttribute attr) : base(attr)
        {
        }

        public override void OnLayoutGUI()
        {
            EditorGUILayout.EndHorizontal();
        }
    }
}
