using DotEngine.Command;
using DotEngine.Interfaces;
using System.Windows;

namespace PureMVCWPF.Controller
{
    public class AddRoleResultCommand : SimpleCommand,ICommand
    {
        public override void Execute(INotification notification)
        {
            bool result = (bool)notification.Body;
            if(!result)
            {
                MessageBox.Show("Role already exists for this user!", "Add User Role");
            }
        }    
    }
}       
