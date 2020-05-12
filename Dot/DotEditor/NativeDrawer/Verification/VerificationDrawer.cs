using Dot.NativeDrawer;

namespace DotEditor.NativeDrawer.Verification
{
    public abstract class VerificationDrawer : CompareAttrNativeDrawer
    {
        protected VerificationDrawer(object target, CompareDrawerAttribute attr) : base(target, attr)
        {
        }

        public abstract void OnLayoutGUI();
    }
}
