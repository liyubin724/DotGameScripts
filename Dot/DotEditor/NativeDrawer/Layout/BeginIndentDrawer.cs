using Dot.NativeDrawer;
using Dot.NativeDrawer.Layout;
using DotEditor.GUIExtension;

namespace DotEditor.NativeDrawer.Layout
{
    [CustomAttDrawerLink(typeof(BeginIndentAttribute))]
    public class BeginIndentDrawer : LayoutDrawer
    {
        public BeginIndentDrawer(NativeDrawerAttribute attr) : base(attr)
        {
        }

        public override void OnLayoutGUI()
        {
            EGUI.BeginIndent();
        }
    }
}
