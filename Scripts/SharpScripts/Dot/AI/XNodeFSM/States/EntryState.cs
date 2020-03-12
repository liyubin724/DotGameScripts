using System;
using XNode;

namespace Dot.AI.XNodeFSM
{
    [NodeTint("#CC6600")]
    [NodeWidth(160)]
    [Serializable]
    public class EntryState : StateBase
    {
        [Output(connectionType = ConnectionType.Override)]
        public StateBase next = null;

        protected internal override void OnEnter(StateBase from)
        {
            NodePort port = GetOutputPort("next");
            if(port!=null && port.Connection!=null)
            {
                Node node = port.Connection.node;
                if(node!=null && node is StateBase state)
                {
                    Machine.ChangeState(state);
                }
            }
        }
    }
}
