using Dot.NativeDrawer.Visible;

namespace DotEditor.NativeDrawer.Visible
{
    [CustomAttDrawerLink(typeof(ShowIfAttribute))]
    public class ShowIfDrawer : VisibleCompareDrawer
    {
        public ShowIfDrawer(object target, VisibleCompareAttribute attr) : base(target, attr)
        {
        }

        public override bool IsVisible()
        {
            bool isShow = IsValid();

            return isShow;
        }
    }
}
