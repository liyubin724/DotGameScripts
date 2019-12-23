using Dot.Lua.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Dot.UI.Panel
{
    public class LuaUIPanel : ChildBindBehaviour
    {
        public Animator animator = null;
        public PanelAnimationTriggers animationTriggers;

        protected override void Awake()
        {
            base.Awake();
            if(animator == null)
            {
                animator = GetComponent<Animator>();
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            animator?.SetTrigger(animationTriggers.OpenTrigger);
        }

        protected override void OnDisable()
        {
            animator?.SetTrigger(animationTriggers.CloseTrigger);
            base.OnDisable();
        }
    }
}
