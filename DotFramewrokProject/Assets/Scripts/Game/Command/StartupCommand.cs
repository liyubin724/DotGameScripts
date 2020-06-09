using DotEngine.Framework;
using DotEngine.Log;
using DotEngine.Net.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Command
{
    public class StartupCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            ClientNetService clientNetService = new ClientNetService();
            AppFacade.GetInstance().RegisterService(ClientNetService.NAME, clientNetService);

            ServerNetService serverNetService = new ServerNetService();
            AppFacade.GetInstance().RegisterService(ServerNetService.NAME, serverNetService);

            AppFacade.GetInstance().RegisterCommand(AppConst.LAUNCH_NET, new LaunchNetCommand());

            LogUtil.LogDebug("StartupCommand", "Startup");
        }
    }
}
