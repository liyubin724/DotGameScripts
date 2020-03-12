using System;

namespace Dot.AI.XNodeFSM
{
    [NodeTint("#990033")]
    [NodeWidth(160)]
    [Serializable]
    public class ExitState : StateBase
    {
        [Input(backingValue = ShowBackingValue.Never)]
        public StateBase prev = null;

        protected internal override void OnEnter(StateBase from)
        {
            Machine.ChangeState(null);
        }
    }
}
