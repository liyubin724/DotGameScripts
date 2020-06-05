using DotEngine.Framework;

namespace PureMVCWPF.View
{
    public class MainViewController : ComplexViewController
    {
        public const string NAME = "MainViewController";

        private MainWindow mainWin = null;

        public MainViewController(MainWindow window)
        {
            mainWin = window;
        }

        public override void OnRegister()
        {
            base.OnRegister();
            AddSubViewController(new UserListViewController(mainWin.userList));
            AddSubViewController(new UserFormViewController(mainWin.userForm));
            AddSubViewController(new RolePanelViewController(mainWin.rolePanel));
        }
    }
}
