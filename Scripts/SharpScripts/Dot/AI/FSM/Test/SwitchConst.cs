using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.AI.FSM.Test
{
    public static class SwitchActionTokens
    {
        public static readonly ActionToken OffActionToken = new ActionToken("OffAction");
        public static readonly ActionToken OnActionToken = new ActionToken("OnAction");
    }

    public static class SwitchStateTokens
    {
        public static readonly StateToken OffStateToken = new StateToken("OffState");
        public static readonly StateToken OnStateToken = new StateToken("OnState");
    }
}
