using DotEngine.Framework;
using PureMVCWPF.Controller;

namespace PureMVCWPF
{
    public class ApplicationFacade : Facade
    {
        public const string STARTUP = "startup";

        public const string NEW_USER = "newUser";
        public const string DELETE_USER = "deleteUser";
        public const string CANCEL_SELECTED = "cancelSelected";

        public const string USER_SELECTED = "userSelected";
        public const string USER_ADDED = "userAdded";
        public const string USER_UPDATED = "userUpdated";
        public const string USER_DELETED = "userDeleted";

        public const string ADD_ROLE = "addRole";
        public const string ADD_ROLE_RESULT = "addRoleResult";

        public new static IFacade GetInstance()
        {
            if(instance == null)
            {
                return instance = new ApplicationFacade();
            }
            return instance;
        }

        public void Startup(object app)
        {
            SendNotification(STARTUP, app);
        }

        static ApplicationFacade() { }

        protected override void InitializeController()
        {
            base.InitializeController();
            RegisterCommand(STARTUP, new StartupCommand());
            RegisterCommand(DELETE_USER, new DeleteUserCommand());
            RegisterCommand(ADD_ROLE_RESULT, new AddRoleResultCommand());
        }
    }
}
