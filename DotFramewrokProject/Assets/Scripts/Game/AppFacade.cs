using DotEngine.Asset;
using DotEngine.Framework;
using DotEngine.GOPool;
using DotEngine.Timer;
using DotEngine.Update;
using DotEngine.Utilities;
using Game.Command;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class AppFacade : Facade
    {
        public static void Startup()
        {
            IFacade facade = AppFacade.GetInstance();
            DontDestroyHandler.CreateComponent<AppFacadeBehaviour>();

            AssetService assetService = facade.RetrieveService<AssetService>(AssetService.NAME);
            assetService.InitDatabaseLoader((result) =>
            {
                facade.SendNotification(AppConst.STARTUP);
            });
        }

        public new static IFacade GetInstance()
        {
            if(instance == null)
            {
                instance = new AppFacade();
            }
            return instance;
        }

        protected override void InitializeFacade()
        {
            base.InitializeFacade();


        }

        protected override void InitializeController()
        {
            base.InitializeController();
            RegisterCommand(AppConst.STARTUP, new StartupCommand());
        }

        protected override void InitializeService()
        {
            base.InitializeService();

            TimerService timerService = new TimerService();
            RegisterService(TimerService.NAME, timerService);

            UpdateService updateService = new UpdateService();
            RegisterService(UpdateService.NAME, updateService);

            AssetService assetService = new AssetService();
            RegisterService(AssetService.NAME, assetService);

            GameObjectPoolService poolService = new GameObjectPoolService(assetService.InstantiateAsset);
            RegisterService(GameObjectPoolService.NAME, poolService);


        }

        protected override void InitializeView()
        {
            base.InitializeView();

            RegisterViewController(StartupViewController.NAME, new StartupViewController());
        }
    }
}


