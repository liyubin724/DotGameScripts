namespace Dot.GUI.Attributes.Property
{
    public class BoxedHeaderAttribute : PropertyAttribute
    {
        public string Header { get; }
        public BoxedHeaderAttribute(string header)
        {
            Header = header;
        }
    }
}
