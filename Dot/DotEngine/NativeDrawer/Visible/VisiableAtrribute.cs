namespace Dot.NativeDrawer.Visible
{
    public abstract class VisiableAtrribute : NativeDrawerAttribute
    {
    }

    public abstract class VisiableCompareAttribute : CompareDrawerAttribute
    {
        protected VisiableCompareAttribute(string memberName, object value, CompareSymbol symbol) : base(memberName, value, symbol)
        {
        }
    }
}
