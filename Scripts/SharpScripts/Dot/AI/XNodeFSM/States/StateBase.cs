using System;
using XNode;
using SystemObject = System.Object;

namespace Dot.AI.XNodeFSM
{
    [NodeTint("#999999")]
    [Serializable]
    public abstract class StateBase : Node
    {
        protected StateMachine Machine { get; private set; }
        public string Name { get => name; }

        protected SystemObject Context
        {
            get
            {
                if(Machine!=null)
                {
                    return Machine.Context;
                }
                return null;
            }
        }

        internal void Initialize(StateMachine machine)
        {
            Machine = machine;
            OnInitialized();
        }

        protected virtual void OnInitialized()
        {
        }

        protected internal virtual void OnEnter(StateBase from)
        {
        }

        protected internal virtual void OnExit(StateBase to)
        {
        }
        
        protected internal virtual void DoUpdate(float deltaTime)
        {
        }
    }
}
