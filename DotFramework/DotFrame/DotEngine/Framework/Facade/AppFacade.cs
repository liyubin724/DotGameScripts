using DotEngine.Framework.Service;
using DotEngine.Framework.Service.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Experimental.PlayerLoop;

namespace DotEngine.Framework.Facade
{
    public class AppFacade : IFacade
    {
        protected static IFacade instance = null;

        public static IFacade GetInstance(Func<IFacade> facadeCreator)
        {
            if(instance == null)
            {
                instance = facadeCreator();
            }
            return instance;
        }

        private ServicerCenter servicerCenter = null;
        private Updater updater = null;

        public Updater GetUpdater()
        {
            return updater;
        }

        public AppFacade()
        {
            if(instance!=null)
            {
                throw new Exception("");
            }
            instance = this;
            InitializeFacade();
        }

        protected virtual void InitializeFacade()
        {
            updater = new Updater();

            InitializeServicerCenter();
            InitializeProxyCenter();
        }

        protected virtual void InitializeServicerCenter()
        {
            servicerCenter = new ServicerCenter();
        }

        protected virtual void InitializeProxyCenter()
        {

        }

        public void DoFixedUpdate(float deltaTime)
        {
            updater?.DoFixedUpdate(deltaTime);
        }

        public void DoLateUpdate(float deltaTime)
        {
            updater?.DoLateUpdate(deltaTime);   
        }

        public void DoUnscaleUpdate(float deltaTime)
        {
            updater?.DoUnscaleUpdate(deltaTime);
        }

        public void DoUpdate(float deltaTime)
        {
            updater?.DoUpdate(deltaTime);
        }
    }
}
