using Dot.NativeDrawer.Decorator;

namespace DotEditor.NativeDrawer.Decorator
{
    public abstract class DecoratorDrawer : AttrNativeDrawer
    {
        protected DecoratorDrawer(DecoratorAttribute attr) : base(attr)
        {
        }

        public abstract void OnGUILayout();
    }
}
