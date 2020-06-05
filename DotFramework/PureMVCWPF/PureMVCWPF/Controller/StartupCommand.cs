using DotEngine.Framework;
using PureMVCWPF.Model;
using PureMVCWPF.View;

namespace PureMVCWPF.Controller
{
    public class StartupCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            Facade.RegisterProxy(UserProxy.NAME, new UserProxy());
            Facade.RegisterProxy(RoleProxy.NAME, new RoleProxy());

            MainWindow window = (MainWindow)notification.Body;
            Facade.RegisterViewController(MainViewController.NAME,new MainViewController(window));
        }
    }
}
