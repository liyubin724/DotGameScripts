using Dot.Dispatch;
using Dot.Lua.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Dot.UI
{
    public class UIManager : LuaScriptBindBehaviour
    {
        public Transform[] uiLayers = new Transform[0];

        protected override void Awake()
        {
        }

        internal void DoInit()
        {
            base.Awake();
        }
    }
}
