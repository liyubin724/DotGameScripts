using DotEngine.Framework;
using PureMVCWPF.Model;
using PureMVCWPF.View;

namespace PureMVCWPF.Controller
{
    public class StartupCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            Facade.RegisterProxy(new UserProxy());
            Facade.RegisterProxy(new RoleProxy());

            MainWindow window = (MainWindow)notification.Body;
            Facade.RegisterViewController(new MainViewController(window));
        }
    }
}
