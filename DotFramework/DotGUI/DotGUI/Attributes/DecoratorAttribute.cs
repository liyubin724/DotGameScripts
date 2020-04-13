namespace Dot.GUI.Attributes
{
    public abstract class DecoratorAttribute : EGUIAttribute
    {
        public int Order { get; set; }
        protected DecoratorAttribute(int order)
        {
            Order = order;
        }
    }
}
