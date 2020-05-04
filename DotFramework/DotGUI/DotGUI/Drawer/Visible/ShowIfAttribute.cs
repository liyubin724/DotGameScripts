using System;

namespace Dot.GUI.Drawer.Visible
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ShowIfAttribute : VisiableCompareAttribute
    {
        public ShowIfAttribute(string comparedName, object comparedValue) : base(comparedName, comparedValue)
        {
        }
    }
}
