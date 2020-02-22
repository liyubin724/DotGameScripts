using Dot.XNodeEx;
using XNodeEditor;

namespace DotEditor.XNodeEx
{
    public class DotNodeEditor : NodeEditor
    {
        public N GetNode<N>() where N : DotNode
        {
            return (N)target;
        }
    }
}
