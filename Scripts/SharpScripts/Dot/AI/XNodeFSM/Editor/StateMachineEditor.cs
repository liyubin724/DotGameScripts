using Dot.AI.XNodeFSM;
using System;
using XNodeEditor;

namespace DotEditor.AI.FSM
{
    [CustomNodeGraphEditor(typeof(StateMachine))]
    public class StateMachineEditor : NodeGraphEditor
    {
        public override string GetNodeMenuName(Type type)
        {
            if(type.FullName.Contains("Dot.AI.FSM"))
            {
                string name = base.GetNodeMenuName(type);
                return name.Replace("Dot/AI/FSM/", "");
            }
            return null;
        }
    }

}
