using UnityEngine;

namespace Dot.AI.FSM.Test
{
    public class SwitchStateMachine : StateMachine
    {
        public SwitchStateMachine():base()
        {
            RegisterState(new OffState());
            RegisterState(new OnState());
        }

        protected override void OnStateChanged(StateChangedEventArgs e)
        {
            Debug.Log($"SwitchStateMachine::OnStateChanged->oldState = {e.OldState},newState={e.NewState}");

            base.OnStateChanged(e);
        }

        public override void DoUpdate(float deltaTime)
        {
            base.DoUpdate(deltaTime);
        }

        protected override void OnComplete()
        {
            Debug.Log($"SwitchStateMachine::OnComplete");
            base.OnComplete();
        }
    }
}
