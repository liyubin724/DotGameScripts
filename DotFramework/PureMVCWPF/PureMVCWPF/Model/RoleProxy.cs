using DotEngine.Framework;
using PureMVCWPF.Model.Enum;
using PureMVCWPF.Model.VO;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PureMVCWPF.Model
{
    public class RoleProxy : Proxy, IProxy
    {
        public new const string NAME = "RoleProxy";

        public RoleProxy() : base(NAME, new ObservableCollection<RoleVO>())
        {
            AddItem(new RoleVO("lstooge",
                new RoleEnum[] { RoleEnum.PAYROLL, RoleEnum.EMP_BENEFITS }));

            AddItem(new RoleVO("cstooge",
                new RoleEnum[] { RoleEnum.ACCT_PAY, RoleEnum.ACCT_RCV, RoleEnum.GEN_LEDGER }));

            AddItem(new RoleVO("mstooge",
                new RoleEnum[] { RoleEnum.INVENTORY, RoleEnum.PRODUCTION, RoleEnum.SALES, RoleEnum.SHIPPING }));
        }

        public IList<RoleVO> Roles
        {
            get { return (IList<RoleVO>)Data; }
        }

        public void AddItem(RoleVO role)
        {
            Roles.Add(role);
        }

        public void DeleteItem(UserVO user)
        {
            for(int i =0;i<Roles.Count;i++)
            {
                if(Roles[i].UserName.Equals(user.UserName))
                {
                    Roles.RemoveAt(i);
                    break;
                }
            }
        }

        public bool DoesUserHaveRole(UserVO user,RoleEnum role)
        {
            bool hasRole = false;
            for(int i =0;i<Roles.Count;++i)
            {
                if(Roles[i].UserName.Equals(user.UserName))
                {
                    IList<RoleEnum> userRoles = Roles[i].Roles;
                    foreach(var curRole in userRoles)
                    {
                        if(curRole == role)
                        {
                            hasRole = true;
                            break;
                        }
                    }
                }
            }
            return hasRole;
        }

        public void AddRoleToUser(UserVO user,RoleEnum role)
        {
            bool result = false;
            if(!DoesUserHaveRole(user,role))
            {
                for(int i =0;i<Roles.Count;++i)
                {
                    if(Roles[i].UserName.Equals(user.UserName))
                    {
                        IList<RoleEnum> userRoles = Roles[i].Roles;
                        userRoles.Add(role);
                        result = true;
                        break;
                    }
                }
            }

            SendNotification(ApplicationFacade.ADD_ROLE, result);
        }

        public void RemoveRoleFromUser(UserVO user,RoleEnum role)
        {
            if(DoesUserHaveRole(user,role))
            {
                for(int i =0;i<Roles.Count;++i)
                {
                    if(Roles[i].UserName.Equals(user.UserName))
                    {
                        foreach(var curRole in Roles[i].Roles)
                        {
                            if(curRole == role)
                            {
                                Roles[i].Roles.Remove(role);
                                break;
                            }
                        }
                    }
                }
            }
        }

        public IList<RoleEnum> GetUserRoles(string userName)
        {
            IList<RoleEnum> userRoles = new ObservableCollection<RoleEnum>();
            for(int i =0;i<Roles.Count;++i)
            {
                if(Roles[i].UserName.Equals(userName))
                {
                    userRoles = Roles[i].Roles;
                    break;
                }
            }
            return userRoles;
        }
    }
}
