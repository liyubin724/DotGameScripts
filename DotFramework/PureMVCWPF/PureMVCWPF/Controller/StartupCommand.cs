using DotEngine.Command;
using DotEngine.Interfaces;
using PureMVCWPF.Model;
using PureMVCWPF.View;

namespace PureMVCWPF.Controller
{
    public class StartupCommand : SimpleCommand,ICommand
    {
        public override void Execute(INotification notification)
        {
            Facade.RegisterProxy(new UserProxy());
            Facade.RegisterProxy(new RoleProxy());

            MainWindow window = (MainWindow)notification.Body;

            Facade.RegisterMediator(new UserFormMediator(window.userForm));
            Facade.RegisterMediator(new UserListMediator(window.userList));
            Facade.RegisterMediator(new RolePanelMediator(window.rolePanel));
        }
    }
}
