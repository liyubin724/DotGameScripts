using PureMVCWPF.Model.Enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PureMVCWPF.Model.VO
{
    public class RoleVO
    {
        private string m_userName;
        public string UserName
        {
            get
            {
                return m_userName;
            }
        }

        private ObservableCollection<RoleEnum> m_roles = new ObservableCollection<RoleEnum>();

        public IList<RoleEnum> Roles
        {
            get { return m_roles; }
        }

        public RoleVO(string un)
        {
            if (un != null) m_userName = un;
        }

        public RoleVO(string userName,RoleEnum[] roles) : this(userName)
        {
            if(roles!=null)
            {
                foreach(var role in roles)
                {
                    m_roles.Add(role);
                }
            }
        }

        public RoleVO(string userName,IList<RoleEnum> roles):this(userName)
        {
            if(roles!=null)
            {
                foreach(var role in roles)
                {
                    m_roles.Add(role);
                }
            }
        }
    }
}
