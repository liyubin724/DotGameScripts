using Dot.NativeDrawer;
using Dot.NativeDrawer.Layout;
using UnityEditor;

namespace DotEditor.NativeDrawer.Layout
{
    [CustomAttDrawerLink(typeof(EndGroupAttribute))]
    public class EndGroupDrawer : LayoutDrawer
    {
        public EndGroupDrawer(NativeDrawerAttribute attr) : base(attr)
        {
        }

        public override void OnLayoutGUI()
        {
            EditorGUILayout.EndVertical();
        }
    }
}
