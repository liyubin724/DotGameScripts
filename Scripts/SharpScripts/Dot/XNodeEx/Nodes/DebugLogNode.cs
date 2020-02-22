namespace Dot.XNodeEx.Nodes
{
    [CreateNodeMenu("Common/Debug Log")]
    public class DebugLogNode : DotNode
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override)]
        public Anything input;

        public object GetValue()
        {
            return GetInputValue<object>("input");
        }

        [System.Serializable] 
        public class Anything { }
    }
}
