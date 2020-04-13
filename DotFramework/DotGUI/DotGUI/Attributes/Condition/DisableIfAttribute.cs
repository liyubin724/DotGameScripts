using System;

namespace Dot.GUI.Attributes.Condition
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class DisableIfAttribute : JudgementConditionAttribute
    {
        public DisableIfAttribute(string comparedMemberName, object comparedValue) : base(comparedMemberName, comparedValue)
        {
        }
    }
}
