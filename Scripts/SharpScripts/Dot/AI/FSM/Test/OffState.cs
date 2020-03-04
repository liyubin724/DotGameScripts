using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Dot.AI.FSM.Test
{
    public class OffState : StateBase
    {
        public OffState() : base(SwitchStateTokens.OffStateToken)
        {
        }

        protected override void OnInitialized()
        {
            RegisterAction(SwitchActionTokens.OnActionToken, OnOnAction);
            base.OnInitialized();
        }

        private void OnOnAction(ActionToken action,object data)
        {
            Debug.Log("OffState::OnOnAction");
            ChangeState(SwitchStateTokens.OnStateToken,data);
        }

        public override void DoUpdate(float deltaTime)
        {
            base.DoUpdate(deltaTime);
        }

        public override void OnEnter(StateEnterEventArgs e)
        {
            base.OnEnter(e);
            Debug.Log("OffState::OnEnter");
        }

        public override void OnExit(StateExitEventArgs e)
        {
            base.OnExit(e);
            Debug.Log("OffState::OnExit");
        }
    }
}
