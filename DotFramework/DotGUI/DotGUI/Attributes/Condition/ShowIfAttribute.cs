using System;

namespace Dot.GUI.Attributes.Condition
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ShowIfAttribute : JudgementConditionAttribute
    {
        public ShowIfAttribute(string comparedMemberName, object comparedValue) : base(comparedMemberName, comparedValue)
        {
        }
    }
}
