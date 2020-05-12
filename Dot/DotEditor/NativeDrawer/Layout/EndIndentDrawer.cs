using Dot.NativeDrawer.Layout;
using DotEditor.GUIExtension;

namespace DotEditor.NativeDrawer.Layout
{
    [CustomAttDrawerLink(typeof(EndIndentAttribute))]
    public class EndIndentDrawer : LayoutDrawer
    {
        public EndIndentDrawer(LayoutAttribute attr) : base(attr)
        {
        }

        public override void OnLayoutGUI()
        {
            EGUI.EndIndent();
        }
    }
}
