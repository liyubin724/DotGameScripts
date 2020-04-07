using System;

namespace Dot.GUI.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class HideIfAttribute : DotPropertyAttribute
    {
        public Func<bool> IsShowFun { get; set; } = null;

        public HideIfAttribute(Func<bool> func):base(AttributePriority.VeryHigh,0)
        {
            IsShowFun = func;
        }
    }
}
