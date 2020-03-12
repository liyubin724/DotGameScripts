using Dot.AI.XNodeFSM;
using UnityEngine;

namespace AI.FSMDemo
{
    public class EatStateHandler : IStateHandler
    {
        private Agent agent = null;
        public void DoEnter(StateBase from)
        {
            Debug.Log($"EatStateHandler::DoEnter->from = {from.Name}!!");
        }

        public void DoExist(StateBase to)
        {
            Debug.Log($"EatStateHandler::DoExist->to = {to.Name}!!");
        }

        public void DoInitilized(object context)
        {
            Debug.Log($"EatStateHandler::DoInitilized->!!");

            agent = (Agent)context;
        }

        public void DoUpdate(float deltaTime)
        {
            if(agent!=null)
            {
                agent.hunger--;
                if(agent.hunger <0)
                {
                    agent.hunger = 0;
                }
            }
        }
    }
}
