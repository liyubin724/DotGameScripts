namespace Dot.GUI.Attributes.Condition
{
    public abstract class JudgementConditionAttribute : ConditionAttribute
    {
        public string ComparedMemberName { get; }
        public object ComparedValue { get;}

        protected JudgementConditionAttribute(string comparedMemberName,object comparedValue)
        {
            ComparedMemberName = comparedMemberName;
            ComparedValue = comparedValue;
        }
    }
}
