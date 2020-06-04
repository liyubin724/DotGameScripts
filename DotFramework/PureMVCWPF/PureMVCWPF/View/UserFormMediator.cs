using DotEngine.Framework;
using PureMVCWPF.Model;
using PureMVCWPF.Model.VO;
using PureMVCWPF.View.Components;
using System;
using System.Collections.Generic;

namespace PureMVCWPF.View
{
    public class UserFormMediator : Mediator,IMediator
    {
        public new const string NAME = "UserFormMediator";

        private UserProxy userProxy;
        private UserForm UserForm
        {
            get { return (UserForm)ViewComponent; }
        }

        public UserFormMediator(UserForm view):base(NAME,view)
        {
            UserForm.AddUser += new EventHandler(UserForm_AddUser);
            UserForm.UpdateUser += new EventHandler(UserForm_UpdateUser);
            UserForm.CancelUser += new EventHandler(UserForm_CancelUser);
        }

        public override void OnRegister()
        {
            base.OnRegister();
            userProxy = (UserProxy)Facade.RetrieveProxy(UserProxy.NAME);
        }

        void UserForm_AddUser(object sender, EventArgs e)
        {
            UserVO user = UserForm.User;
            userProxy.AddItem(user);
            SendNotification(ApplicationFacade.USER_ADDED, user);
            UserForm.ClearForm();
        }

        void UserForm_UpdateUser(object sender, EventArgs e)
        {
            UserVO user = UserForm.User;
            userProxy.UpdateItem(user);
            SendNotification(ApplicationFacade.USER_UPDATED, user);
            UserForm.ClearForm();
        }

        void UserForm_CancelUser(object sender, EventArgs e)
        {
            SendNotification(ApplicationFacade.CANCEL_SELECTED);
            UserForm.ClearForm();
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
                    UserForm.ShowUser(user, UserFormMode.ADD);
                    break;

                case ApplicationFacade.USER_DELETED:
                    UserForm.ClearForm();
                    break;

                case ApplicationFacade.USER_SELECTED:
                    user = (UserVO)note.Body;
                    UserForm.ShowUser(user, UserFormMode.EDIT);
                    break;

            }
        }
    }
}
