namespace Dot.GUI.Drawer.Condition
{
    public abstract class CompareAttribute : ConditionDrawerAttribute
    {
        public string ComparedName { get; private set; }
        public object ComparedValue { get; private set; }

        protected CompareAttribute(string comparedName,object comparedValue)
        {
            ComparedName = comparedName;
            ComparedValue = comparedValue;
        }
    }
}
