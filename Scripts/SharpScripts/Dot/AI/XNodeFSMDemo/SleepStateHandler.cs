using Dot.AI.XNodeFSM;
using UnityEngine;

namespace AI.FSMDemo
{
    public class SleepStateHandler : IStateHandler
    {
        private Agent agent = null;
        public void DoEnter(StateBase from)
        {
            Debug.Log($"SleepStateHandler::DoEnter->from = {from.Name}!!");
        }

        public void DoExist(StateBase to)
        {
            Debug.Log($"SleepStateHandler::DoExist->to = {to.Name}!!");
        }

        public void DoInitilized(object context)
        {
            Debug.Log($"SleepStateHandler::DoInitilized->!!");

            agent = (Agent)context;
        }

        public void DoUpdate(float deltaTime)
        {
            if (agent != null)
            {
                agent.fatige--;
                if (agent.fatige < 0)
                {
                    agent.fatige = 0;
                }
            }
        }
    }
}
