using UnityEngine;

namespace Dot.AI.FSM.Test
{
    public class OnState : StateBase
    {
        public OnState() : base(SwitchStateTokens.OnStateToken)
        {
        }

        protected override void OnInitialized()
        {
            RegisterAction(SwitchActionTokens.OffActionToken, OnOffAction);
            base.OnInitialized();
        }

        private void OnOffAction(ActionToken action, object data)
        {
            Debug.Log("OnState::OnOffAction");
            ChangeState(SwitchStateTokens.OffStateToken, data);
        }

        public override void DoUpdate(float deltaTime)
        {
            base.DoUpdate(deltaTime);
        }

        public override void OnEnter(StateEnterEventArgs e)
        {
            base.OnEnter(e);
            Debug.Log("OnState::OnEnter");
        }

        public override void OnExit(StateExitEventArgs e)
        {
            base.OnExit(e);
            Debug.Log("OnState::OnExit");
        }
    }
}
