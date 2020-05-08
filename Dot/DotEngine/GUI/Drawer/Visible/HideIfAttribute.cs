using System;

namespace Dot.GUI.Drawer.Visible
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class HideIfAttribute : VisiableCompareAttribute
    {
        public HideIfAttribute(string comparedName, object comparedValue) : base(comparedName, comparedValue)
        {
        }
    }
}
