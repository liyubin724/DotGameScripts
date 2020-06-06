using DotEngine.Framework;
using DotEngine.Log;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Command
{
    public class StartupCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            LogUtil.LogDebug("StartupCommand", "Startup");
        }
    }
}
