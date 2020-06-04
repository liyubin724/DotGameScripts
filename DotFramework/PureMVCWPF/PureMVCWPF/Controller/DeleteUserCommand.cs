using DotEngine.Command;
using DotEngine.Interfaces;
using PureMVCWPF.Model;
using PureMVCWPF.Model.VO;

namespace PureMVCWPF.Controller
{
    public class DeleteUserCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            UserVO user = (UserVO)notification.Body;
            UserProxy userProxy = (UserProxy)Facade.RetrieveProxy(UserProxy.NAME);
            RoleProxy roleProxy = (RoleProxy)Facade.RetrieveProxy(RoleProxy.NAME);
            userProxy.DeleteItem(user);
            roleProxy.DeleteItem(user);

            SendNotification(ApplicationFacade.USER_DELETED);
        }
    }
}
