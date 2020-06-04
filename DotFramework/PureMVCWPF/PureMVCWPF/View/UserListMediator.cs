using DotEngine.Interfaces;
using DotEngine.Patterns.Mediator;
using PureMVCWPF.Model;
using PureMVCWPF.Model.VO;
using PureMVCWPF.View.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace PureMVCWPF.View
{
    public class UserListMediator : Mediator,IMediator
    {
        private UserProxy userProxy;

        public new const string NAME = "UserListMediator";

        public UserListMediator(UserList userList)
            : base(NAME, userList)
        {
            userList.NewUser += new EventHandler(userList_NewUser);
            userList.DeleteUser += new EventHandler(userList_DeleteUser);
            userList.SelectUser += new EventHandler(userList_SelectUser);
        }

        public override void OnRegister()
        {
            userProxy = (UserProxy)Facade.RetrieveProxy(UserProxy.NAME);
            UserList.LoadUsers(userProxy.Users);
        }

        private UserList UserList
        {
            get { return (UserList)ViewComponent; }
        }

        void userList_NewUser(object sender, EventArgs e)
        {
            UserVO user = new UserVO();
            SendNotification(ApplicationFacade.NEW_USER, user);
        }

        void userList_DeleteUser(object sender, EventArgs e)
        {
            SendNotification(ApplicationFacade.DELETE_USER, UserList.SelectedUser);
        }

        void userList_SelectUser(object sender, EventArgs e)
        {
            SendNotification(ApplicationFacade.USER_SELECTED, UserList.SelectedUser);
        }

        public override string[] ListNotificationInterests()
        {
            List<string> list = new List<string>();
            list.Add(ApplicationFacade.CANCEL_SELECTED);
            list.Add(ApplicationFacade.USER_UPDATED);
            return list.ToArray();
        }

        public override void HandleNotification(INotification note)
        {
            switch (note.Name)
            {
                case ApplicationFacade.CANCEL_SELECTED:
                    UserList.Deselect();
                    break;

                case ApplicationFacade.USER_UPDATED:
                    UserList.Deselect();
                    break;
            }
        }
    }
}
