﻿using System.Text;
using UnityEngine;

namespace Dot.Utility
{
    public static class TransformUtility
    {
        public static Transform GetChildByName(this Transform tran, string name)
        {
            if (tran.name == name)
            {
                return tran;
            }

            for (int i = 0; i < tran.childCount; ++i)
            {
                Transform target = GetChildByName(tran.GetChild(i), name);
                if (target != null)
                {
                    return target;
                }
            }

            return null;
        }

        public static string GetPath(this Transform tran)
        {
            StringBuilder nameSB = new StringBuilder();
            Transform parent = tran;
            while(parent != null)
            {
                if(nameSB.Length>0)
                {
                    nameSB.Insert(0, "/");
                }
                nameSB.Insert(0, parent.name);
                parent = tran.parent;
            }
            return nameSB.ToString();
        }
    }
}
