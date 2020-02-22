using XNode;

namespace Dot.XNodeEx
{
    public abstract class DotNode : Node
    {
        public T GetGraph<T>() where T : DotNodeGraph
        {
            return (T)graph;
        }

        public virtual void OnWillBeAdd()
        {

        }

        public virtual void OnWillBeRemove()
        {

        }
    }
}
