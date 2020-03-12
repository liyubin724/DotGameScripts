using System;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Dot.AI.XNodeFSM
{
    [NodeTint("#999999")]
    [Serializable]
    [NodeWidth(260)]
    public class AgentState : StateBase
    {
        [Input(ShowBackingValue.Never)]
        public StateBase prev = null;

        [Space]
        public string handler = null;

        [Space]
        [Output(dynamicPortList = true)]
        public List<StateCondition> conditions = new List<StateCondition>();

        [Serializable]
        public class StateCondition
        {
            public string method;
            public string arg = null;
        }

        private IStateHandler handlerObj = null;
        protected override void OnInitialized()
        {
            if(!string.IsNullOrEmpty(handler))
            {
                handlerObj = StateHanlderCache.GetStateHandler(handler);
                handlerObj?.DoInitilized(Context);
            }
        }

        protected internal override void OnEnter(StateBase from)
        {
            handlerObj?.DoEnter(from);
        }

        protected internal override void OnExit(StateBase to)
        {
            handlerObj?.DoExist(to);
        }

        protected internal override void DoUpdate(float deltaTime)
        {
            handlerObj?.DoUpdate(deltaTime);

            if(conditions.Count>0)
            {
                for(int i =0;i<conditions.Count;++i)
                {
                    StateCondition condition = conditions[i];
                    if(condition!=null && Machine.GetConditionResult(condition.method,condition.arg))
                    {
                        string portName = $"conditions {i}";
                        NodePort port = GetOutputPort(portName);
                        if(port!=null && port.Connection!=null && port.Connection.node!=null)
                        {
                            StateBase nextState = port.Connection.node as StateBase;
                            if(nextState!=null)
                            {
                                Machine.ChangeState(nextState);
                                break;
                            }
                        }
                    }
                }
            }
        }

    }
}
