using System;

namespace Dot.GUI.Attributes.Condition
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class HideIfAttribute : JudgementConditionAttribute
    {
        public HideIfAttribute(string comparedMemberName, object comparedValue) : base(comparedMemberName, comparedValue)
        {
        }
    }
}
