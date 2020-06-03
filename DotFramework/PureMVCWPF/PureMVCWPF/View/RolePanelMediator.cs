﻿using DotMVCS.Interfaces;
using DotMVCS.Patterns.Mediator;
using PureMVCWPF.Model;
using PureMVCWPF.Model.VO;
using PureMVCWPF.View.Components;
using System;
using System.Collections.Generic;

namespace PureMVCWPF.View
{
    public class RolePanelMediator : Mediator,IMediator
    {
        public new const string NAME = "RolePanelMediator";

        private RoleProxy roleProxy;
        private RolePanel RolePanel
        {
            get { return (RolePanel)ViewComponent; }
        }


        public RolePanelMediator(RolePanel viewComponent):base(NAME,viewComponent)
        {
            RolePanel.AddRole += new EventHandler(RolePanel_AddRole);
            RolePanel.RemoveRole += new EventHandler(RolePanel_RemoveRole);
        }

        public override void OnRegister()
        {
            base.OnRegister();
            roleProxy = (RoleProxy)Facade.RetrieveProxy(RoleProxy.NAME);
        }

        public override void OnRemove()
        {
            base.OnRemove();
        }

        void RolePanel_RemoveRole(object sender, EventArgs e)
        {
            roleProxy.RemoveRoleFromUser(RolePanel.User, RolePanel.SelectedRole);
        }

        void RolePanel_AddRole(object sender, EventArgs e)
        {
            roleProxy.AddRoleToUser(RolePanel.User, RolePanel.SelectedRole);
        }

        public override string[] ListNotificationInterests()
        {
            List<string> list = new List<string>();
            list.Add(ApplicationFacade.NEW_USER);
            list.Add(ApplicationFacade.USER_ADDED);
            list.Add(ApplicationFacade.USER_DELETED);
            list.Add(ApplicationFacade.CANCEL_SELECTED);
            list.Add(ApplicationFacade.USER_SELECTED);
            list.Add(ApplicationFacade.ADD_ROLE_RESULT);
            return list.ToArray();
        }


        public override void HandleNotification(INotification note)
        {
            UserVO user;
            RoleVO role;
            string userName;

            switch(note.Name)
            {
                case ApplicationFacade.NEW_USER:
                    RolePanel.ClearForm();
                    break;
                case ApplicationFacade.USER_ADDED:
                    user = (UserVO)note.Body;
                    userName = user == null ? "" : user.UserName;
                    role = new RoleVO(userName);
                    roleProxy.AddItem(role);
                    RolePanel.ClearForm();
                    break;
                case ApplicationFacade.USER_UPDATED:
                    RolePanel.ClearForm();
                    break;
                case ApplicationFacade.USER_DELETED:
                    RolePanel.ClearForm();
                    break;
                case ApplicationFacade.CANCEL_SELECTED:
                    RolePanel.ClearForm();
                    break;
                case ApplicationFacade.USER_SELECTED:
                    user = (UserVO)note.Body;
                    userName = user == null ? "" : user.UserName;
                    RolePanel.ShowUser(user, roleProxy.GetUserRoles(userName));
                    break;
                case ApplicationFacade.ADD_ROLE_RESULT:
                    userName = RolePanel.User == null ? "" : RolePanel.User.UserName;
                    RolePanel.ShowUserRoles(roleProxy.GetUserRoles(userName));
                    break;
            }

        }

    }
}
