using Dot.AI.XNodeFSM;
using System;
using UnityEngine;

namespace AI.FSMDemo
{
    public class WorkStateHandler : IStateHandler
    {
        private Agent agent = null;
        public void DoEnter(StateBase from)
        {
            Debug.Log($"WorkStateHandler::DoEnter->from = {from.Name}!!");
        }

        public void DoExist(StateBase to)
        {
            Debug.Log($"WorkStateHandler::DoExist->to = {to.Name}!!");
        }

        public void DoInitilized(object context)
        {
            Debug.Log($"WorkStateHandler::DoInitilized->!!");

            agent = (Agent)context;
        }

        public void DoUpdate(float deltaTime)
        {
            if (agent != null)
            {
                UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
                int random = UnityEngine.Random.Range(0, 2);
                if(random == 0)
                {
                    agent.fatige++;
                }else
                {
                    agent.hunger++;
                }

                agent.wealth++;
            }
        }
    }
}
