using System;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using SystemObject = System.Object;

namespace Dot.AI.XNodeFSM
{
    [CreateAssetMenu(fileName = "machine_graph", menuName = "State Machine")]
    public class StateMachine : NodeGraph
    {
        internal SystemObject Context { get; private set; }

        private IMachineHandler handler = null;
        private StateBase currentState = null;
        private Dictionary<string, Func<string,bool>> conditionArgFuncDic = new Dictionary<string, Func<string,bool>>();

        public bool IsRunning { get => currentState != null; }
        public bool IsRuntimeAsset { get; set; } = false;

        public void SetEnv(IMachineHandler machineHandler,SystemObject context)
        {
            handler = machineHandler;
            Context = context;

            foreach(var node in nodes)
            {
                if(node!=null && node is StateBase state)
                {
                    state.Initialize(this);
                }
            }

            EntryState entry = GetStart();
            if(entry!=null)
            {
                ChangeState(entry);
            }
        }

        private EntryState GetStart()
        {
            foreach (var node in nodes)
            {
                if (node != null && node is EntryState)
                {
                    return node as EntryState;
                }
            }
            return null;
        }

        public void DoUpdate(float deltaTime)
        {
            currentState?.DoUpdate(deltaTime);
        }

        internal void ChangeState(StateBase state)
        {
            if(state==currentState)
            {
                return;
            }

            StateBase oldState = currentState;
            StateBase newState = state;

            if (newState == null)
            {
                if(IsRunning)
                {
                    currentState.OnExit(null);
                    currentState = null;
                }
                handler?.OnMachineComplete(this);
            }
            else
            {
                if(IsRunning)
                {
                    currentState.OnExit(newState);
                    currentState = newState;
                    currentState.OnEnter(oldState);

                    handler?.OnStateChanged(this, oldState, newState);
                }
                else
                {
                    currentState = newState;
                    currentState.OnEnter(null);
                }
            }
        }

        internal bool GetConditionResult(string methodName,string argValue)
        {
            if(string.IsNullOrEmpty(methodName))
            {
                Debug.LogError("");
                return false;
            }

            if (!conditionArgFuncDic.TryGetValue(methodName, out Func<string, bool> func))
            {
                func = handler?.GetArgCondition(methodName);
                conditionArgFuncDic.Add(methodName, func);
            }
            if (func != null)
            {
                return func(argValue);
            }
            else
            {
                Debug.LogError("");
            }

            return false;
        }

        protected override void OnDestroy()
        {
            if(!IsRuntimeAsset)
            {
                base.OnDestroy();
            }
        }
    }
}
