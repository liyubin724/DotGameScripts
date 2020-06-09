using DotEngine.Asset;
using DotEngine.Framework;
using UnityEngine;

namespace Game
{
    public class StartupViewController : SingleViewController
    {
        public const string NAME = "StartupViewController";

        private GameObject gObj = null;
        public override string[] ListNotificationInterests()
        {
            return new string[]
            {
                AppConst.STARTUP,
            };
        }

        public override void HandleNotification(INotification notification)
        {
            if(notification.Name == AppConst.STARTUP)
            {
                OnStartup();
            }
        }

        private void OnStartup()
        {
            AssetService assetService = AppFacade.GetInstance().RetrieveService<AssetService>(AssetService.NAME);
            assetService.InstanceAssetAsync("Cube.prefab", (address,gObj,userData) =>
            {
                gObj = (GameObject)gObj;
                SendNotification(AppConst.LAUNCH_NET);
            });
        }
    }
}
