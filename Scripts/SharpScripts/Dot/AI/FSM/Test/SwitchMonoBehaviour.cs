using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Dot.AI.FSM.Test
{
    public class SwitchMonoBehaviour : MonoBehaviour
    {
        private SwitchStateMachine machine = null;

        private void OnGUI()
        {
            if(GUILayout.Button("Init Machine"))
            {
                machine = new SwitchStateMachine();
                machine.SetInitialState(SwitchStateTokens.OffStateToken);
            }

            if(GUILayout.Button("Off"))
            {
                machine.PerformAction(SwitchActionTokens.OffActionToken);
            }
            if(GUILayout.Button("On"))
            {
                machine.PerformAction(SwitchActionTokens.OnActionToken);
            }

            if(GUILayout.Button("Cancel"))
            {
                machine.PerformTransitionTo(null);
            }
        }

        private void Update()
        {
            machine?.DoUpdate(Time.deltaTime);
        }
    }
}
