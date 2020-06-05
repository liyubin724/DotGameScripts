using DotEngine.Framework;
using DotEngine.Timer;
using DotEngine.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine
{
    public class AppFacade : Facade
    {

        public new static IFacade GetInstance()
        {
            if(instance == null)
            {
                instance = new AppFacade();
            }
            return instance;
        }

        private AppFacade() { }

        protected override void InitializeService()
        {
            base.InitializeService();

            RegisterService(AppServiceConst.TIMER_SERVICE_NAME,new TimerService());
            RegisterService(AppServiceConst.UPDATE_SERVICE_NAME, new UpdateService());

        }
    }
}
