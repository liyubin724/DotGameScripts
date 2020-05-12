using System;

namespace Dot.NativeDrawer.Visible
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ShowIfAttribute : VisiableCompareAttribute
    {
        public ShowIfAttribute(string memberName) : base(memberName, true, CompareSymbol.Eq)
        {
        }
    }
}
