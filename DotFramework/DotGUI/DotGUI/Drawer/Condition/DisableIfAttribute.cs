using System;

namespace Dot.GUI.Drawer.Condition
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class DisableIfAttribute : CompareAttribute
    {
        public DisableIfAttribute(string comparedName, object comparedValue) : base(comparedName, comparedValue)
        {
        }
    }
}
