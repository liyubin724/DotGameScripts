using DotEngine.Framework.Services;
using DotEngine.Framework.Services.Update;
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

        private IServiceCenter servicerCenter = null;

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
            InitializeServicerCenter();
            InitializeProxyCenter();
        }

        protected virtual void InitializeServicerCenter()
        {
            servicerCenter = new ServiceCenter();
            servicerCenter.Register(ServiceConst.UpdateServiceName, new UpdateService());
        }

        protected virtual void InitializeProxyCenter()
        {

        }

        public void DoFixedUpdate(float deltaTime)
        {
            servicerCenter.DoFixedUpdate(deltaTime);
        }

        public void DoLateUpdate(float deltaTime)
        {
            servicerCenter.DoLateUpdate(deltaTime);   
        }

        public void DoUnscaleUpdate(float deltaTime)
        {
            servicerCenter.DoUnscaleUpdate(deltaTime);
        }

        public void DoUpdate(float deltaTime)
        {
            servicerCenter.DoUpdate(deltaTime);
        }
    }
}
