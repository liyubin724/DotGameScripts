using System.Windows;

namespace PureMVCWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow window = new MainWindow();
            ApplicationFacade facade = (ApplicationFacade)ApplicationFacade.Instance;
            facade.Startup(window);
            window.Show();
        }
    }
}
