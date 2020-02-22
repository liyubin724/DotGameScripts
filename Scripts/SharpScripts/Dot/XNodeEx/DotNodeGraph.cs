using Dot.Env;
using System;
using System.Collections.Generic;
using XNode;

namespace Dot.XNodeEx
{
    public class DotNodeGraph : NodeGraph
    {
        private Context context = null;
        public Context GraphContext
        {
            get
            {
                if(context == null)
                {
                    context = new Context();
                }
                return context;
            }
        }

        public override Node AddNode(Type type)
        {
            Node node = base.AddNode(type);
            if(node !=null && type.IsAssignableFrom(typeof(DotNode)))
            {
                DotNode dNode = node as DotNode;
                dNode.OnWillBeAdd();
            }
            return node;
        }

        public override void RemoveNode(Node node)
        {
            if (node != null && node.GetType().IsAssignableFrom(typeof(DotNode)))
            {
                DotNode dNode = node as DotNode;
                dNode.OnWillBeRemove();
            }
            base.RemoveNode(node);
        }

        public T GetNode<T>() where T : DotNode
        {
            foreach( var node in nodes)
            {
                if(node!=null && node is T)
                {
                    return (T)node;
                }
            }

            return null;
        }

        public T[] GetNodes<T>() where T: DotNode
        {
            List<T> list = new List<T>();
            foreach(var node in nodes)
            {
                if(node!=null && node is T)
                {
                    list.Add((T)node);
                }
            }
            return list.ToArray();
        }
    }
}
