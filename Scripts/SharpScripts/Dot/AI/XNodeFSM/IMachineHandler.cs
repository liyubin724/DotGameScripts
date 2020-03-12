using System;

namespace Dot.AI.XNodeFSM
{
    public interface IMachineHandler
    {
        Func<string, bool> GetArgCondition(string name);

        void OnStateChanged(StateMachine machine,StateBase from,StateBase to);

        void OnMachineComplete(StateMachine machine);
    }
}
