using Dot.NativeDrawer.Visible;

namespace DotEditor.NativeDrawer.Visible
{
    [CustomAttDrawerLink(typeof(ShowIfAttribute))]
    public class ShowIfDrawer : ConditionVisibleDrawer
    {
        public ShowIfDrawer(object target, VisiableCompareAttribute attr) : base(target, attr)
        {
        }

        public override bool IsVisible()
        {
            return IsValid();
        }
    }
}
