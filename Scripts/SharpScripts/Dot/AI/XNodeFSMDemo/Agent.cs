using Dot.AI.XNodeFSM;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace AI.FSMDemo
{
    public class Agent : IMachineHandler
    {
        public int fatige = 0;
        public int hunger = 0;
        public int wealth = 0;

        private Dictionary<string, Func<string,bool>> conditionFuncDic = new Dictionary<string, Func<string, bool>>();
        private StateMachine runningMachine = null;

        public Agent()
        {
            conditionFuncDic.Add("IsWealthEnough", IsWealthEnough);
            conditionFuncDic.Add("IsSleepToEat", IsSleepToEat);
            conditionFuncDic.Add("IsSleepToWork", IsSleepToWork);
            conditionFuncDic.Add("IsEatToSleep", IsEatToSleep);
            conditionFuncDic.Add("IsEatToWork", IsEatToWork);
            conditionFuncDic.Add("IsWorkToEat", IsWorkToEat);
            conditionFuncDic.Add("IsWorkToSleep", IsWorkToSleep);
        }

        public void StartFSM(StateMachine machine)
        {
            runningMachine = UnityObject.Instantiate<StateMachine>(machine);
            runningMachine.IsRuntimeAsset = true;
            runningMachine.SetEnv(this, this);
        }

        public void DoUpdate(float deltaTime)
        {
            runningMachine?.DoUpdate(deltaTime);
        }

        private bool IsWealthEnough(string arg)
        {
            return wealth >= 100;
        }

        private bool IsSleepToEat(string arg)
        {
            return fatige == 0 && hunger >= 100;
        }

        private bool IsSleepToWork(string arg)
        {
            return fatige == 0 && hunger <100;
        }

        private bool IsEatToSleep(string arg)
        {
            return hunger == 0 && fatige >= 100;
        }

        private bool IsEatToWork(string arg)
        {
            return hunger == 0 && fatige < 100;
        }

        private bool IsWorkToEat(string arg)
        {
            return wealth < 100 && hunger >= 100;
        }

        private bool IsWorkToSleep(string arg)
        {
            return wealth < 100 && fatige >= 100;
        }

        public Func<string, bool> GetArgCondition(string name)
        {
            return conditionFuncDic[name];
        }

        public Func<bool> GetCondition(string name)
        {
            throw new NotImplementedException();
        }

        public void OnMachineComplete(StateMachine machine)
        {
            Debug.Log("Agen::OnMachineComplete->machine is complete!!");
            runningMachine = null;
        }

        public void OnStateChanged(StateMachine machine, StateBase from, StateBase to)
        {
            Debug.Log($"Agen::OnStateChanged->from = {(from==null?"null":from.Name)},to = {(to==null?"null":to.Name)}!!");
        }
    }
}
