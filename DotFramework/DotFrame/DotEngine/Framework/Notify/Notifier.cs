using DotEngine.Framework.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Framework.Notify
{
    public class Notifier : INotifier
    {
        public void SendNotification(string notificationName, object body = null)
        {
            throw new NotImplementedException();
        }

        //protected INotifyService NotifyService
        //{
        //    get
        //    {
        //        return AppFacade.GetInstance()
        //    }
        //}
    }
}
