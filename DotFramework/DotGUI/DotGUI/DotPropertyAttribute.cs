using UnityEngine;

namespace Dot.GUI
{
    public enum AttributePriority
    {
        VeryLow =1,
        Low,
        Default,
        High,
        VeryHigh,
    }

    public abstract class DotPropertyAttribute : PropertyAttribute
    {
        public bool IsEnableWhenHide { get; set; } = false;
        protected DotPropertyAttribute(AttributePriority priority,int order)
        {
            if(order<0)
            {
                order = 0;
            }else if(order> 99999)
            {
                order = 99999;
            }
            base.order = (int)priority * 100000 + order;
        }

        protected DotPropertyAttribute():this(AttributePriority.Default,0)
        {
        }
    }
}
