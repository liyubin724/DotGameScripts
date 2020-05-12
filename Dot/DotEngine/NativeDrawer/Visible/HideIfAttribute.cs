using System;

namespace Dot.NativeDrawer.Visible
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class HideIfAttribute : VisiableCompareAttribute
    {
        public HideIfAttribute(string memberName) : base(memberName, true, CompareSymbol.Eq)
        {
        }
    }
}
