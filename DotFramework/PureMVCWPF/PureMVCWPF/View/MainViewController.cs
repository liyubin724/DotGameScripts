using DotEngine.Framework;

namespace PureMVCWPF.View
{
    public class MainViewController : ComplexViewController
    {
        private MainWindow mainWin = null;

        public MainViewController(MainWindow window) : base("MainViewController")
        {
            mainWin = window;
        }

        public override void OnRegister()
        {
            base.OnRegister();
            AddSubController(new UserListViewController(mainWin.userList));
            AddSubController(new UserFormViewController(mainWin.userForm));
            AddSubController(new RolePanelViewController(mainWin.rolePanel));
        }
    }
}
