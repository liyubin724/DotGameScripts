using DotEngine.Framework;
using PureMVCWPF.Model;
using PureMVCWPF.Model.VO;

namespace PureMVCWPF.Controller
{
    public class DeleteUserCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            UserVO user = (UserVO)notification.Body;
            UserProxy userProxy = Facade.RetrieveProxy<UserProxy>(UserProxy.NAME);
            RoleProxy roleProxy = Facade.RetrieveProxy<RoleProxy>(RoleProxy.NAME);
            userProxy.DeleteItem(user);
            roleProxy.DeleteItem(user);

            SendNotification(ApplicationFacade.USER_DELETED);
        }
    }
}
