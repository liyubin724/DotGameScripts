using DotEngine.Framework;
using PureMVCWPF.Model;
using PureMVCWPF.Model.VO;
using PureMVCWPF.View.Components;
using System;
using System.Collections.Generic;

namespace PureMVCWPF.View
{
    public class UserFormViewController : SingleViewController,IViewController
    {
        public const string NAME = "UserFormMediator";

        private UserProxy userProxy;
        private UserForm userForm;

        public UserFormViewController(UserForm view):base(NAME)
        {
            userForm = view;

            userForm.AddUser += new EventHandler(UserForm_AddUser);
            userForm.UpdateUser += new EventHandler(UserForm_UpdateUser);
            userForm.CancelUser += new EventHandler(UserForm_CancelUser);
        }

        public override void OnRegister()
        {
            base.OnRegister();
            userProxy = (UserProxy)Facade.RetrieveProxy(UserProxy.NAME);
        }

        void UserForm_AddUser(object sender, EventArgs e)
        {
            UserVO user = userForm.User;
            userProxy.AddItem(user);
            SendNotification(ApplicationFacade.USER_ADDED, user);
            userForm.ClearForm();
        }

        void UserForm_UpdateUser(object sender, EventArgs e)
        {
            UserVO user = userForm.User;
            userProxy.UpdateItem(user);
            SendNotification(ApplicationFacade.USER_UPDATED, user);
            userForm.ClearForm();
        }

        void UserForm_CancelUser(object sender, EventArgs e)
        {
            SendNotification(ApplicationFacade.CANCEL_SELECTED);
            userForm.ClearForm();
        }

        public override string[] ListNotificationInterests()
        {
            List<string> list = new List<string>();
            list.Add(ApplicationFacade.NEW_USER);
            list.Add(ApplicationFacade.USER_DELETED);
            list.Add(ApplicationFacade.USER_SELECTED);
            return list.ToArray();
        }

        public override void HandleNotification(INotification note)
        {
            UserVO user;

            switch (note.Name)
            {
                case ApplicationFacade.NEW_USER:
                    user = (UserVO)note.Body;
                    userForm.ShowUser(user, UserFormMode.ADD);
                    break;

                case ApplicationFacade.USER_DELETED:
                    userForm.ClearForm();
                    break;

                case ApplicationFacade.USER_SELECTED:
                    user = (UserVO)note.Body;
                    userForm.ShowUser(user, UserFormMode.EDIT);
                    break;

            }
        }
    }
}
