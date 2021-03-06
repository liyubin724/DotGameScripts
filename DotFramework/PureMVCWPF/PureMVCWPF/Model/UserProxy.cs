﻿using DotEngine.Framework;
using PureMVCWPF.Model.Enum;
using PureMVCWPF.Model.VO;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PureMVCWPF.Model
{
    public class UserProxy : Proxy, IProxy
    {
        public const string NAME = "UserProxy";

        private ObservableCollection<UserVO> users = null;
        public UserProxy()
        {
            users = new ObservableCollection<UserVO>();

            AddItem(new UserVO("lstooge", "Larry", "Stooge", "larry@stooges.com", "ijk456", DeptEnum.ACCT));
            AddItem(new UserVO("cstooge", "Curly", "Stooge", "curly@stooges.com", "xyz987", DeptEnum.SALES));
            AddItem(new UserVO("mstooge", "Moe", "Stooge", "moe@stooges.com", "abc123", DeptEnum.PLANT));
        }

        public IList<UserVO> Users
        {
            get
            {
                return users;
            }
        }

        public void AddItem(UserVO user)
        {
            Users.Add(user);
        }

        public void UpdateItem(UserVO user)
        {
            for(int i =0;i<Users.Count;i++)
            {
                if(Users[i].UserName.Equals(user.UserName))
                {
                    Users[i]= user;
                    return;
                }
            }
        }

        public void DeleteItem(UserVO user)
        {
            for(int i =0;i<Users.Count;++i)
            {
                if(Users[i].UserName.Equals(user.UserName))
                {
                    Users.RemoveAt(i);
                    break;
                }
            }
        }

    }
}
