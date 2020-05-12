using Dot.NativeDrawer;
using Dot.NativeDrawer.Decorator;

namespace DotEditor.NativeDrawer.Decorator
{
    public abstract class DecoratorDrawer : AttrNativeDrawer
    {
        protected DecoratorDrawer(NativeDrawerAttribute attr) : base(attr)
        {
        }

        public abstract void OnLayoutGUI();
    }
}
