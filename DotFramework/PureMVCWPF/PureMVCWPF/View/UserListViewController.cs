using DotEngine.Framework;
using PureMVCWPF.Model;
using PureMVCWPF.Model.VO;
using PureMVCWPF.View.Components;
using System;
using System.Collections.Generic;

namespace PureMVCWPF.View
{
    public class UserListViewController : SingleViewController,IViewController
    {

        public const string NAME = "UserListMediator";

        private UserProxy userProxy;
        private UserList userList;

        public UserListViewController(UserList userList)
        {
            this.userList = userList;

            userList.NewUser += new EventHandler(userList_NewUser);
            userList.DeleteUser += new EventHandler(userList_DeleteUser);
            userList.SelectUser += new EventHandler(userList_SelectUser);
        }

        public override void OnRegister()
        {
            userProxy = Facade.RetrieveProxy<UserProxy>(UserProxy.NAME);
            userList.LoadUsers(userProxy.Users);
        }

        

        void userList_NewUser(object sender, EventArgs e)
        {
            UserVO user = new UserVO();
            SendNotification(ApplicationFacade.NEW_USER, user);
        }

        void userList_DeleteUser(object sender, EventArgs e)
        {
            SendNotification(ApplicationFacade.DELETE_USER, userList.SelectedUser);
        }

        void userList_SelectUser(object sender, EventArgs e)
        {
            SendNotification(ApplicationFacade.USER_SELECTED, userList.SelectedUser);
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
                    userList.Deselect();
                    break;

                case ApplicationFacade.USER_UPDATED:
                    userList.Deselect();
                    break;
            }
        }
    }
}
