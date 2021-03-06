﻿using System.Linq;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace Dot.Asset
{
    public class AssetHandler
    {
        public string Label { get; private set; }
        public string[] Addresses { get; private set; }
        public string Address
        {
            get
            {
                if(Addresses!=null && Addresses.Length>0)
                {
                    return Addresses[0];
                }
                return null;
            }
        }
        public UnityObject[] UObjects { get; internal set; }
        public UnityObject UObject
        {
            get
            {
                if(UObjects!=null && UObjects.Length>0)
                {
                    return UObjects[0];
                }
                return null;
            }
        }

        public float[] Progresses { get; internal set; }
        public float Progress { 
            get
            {
                if(Progresses!=null && Progresses.Length>0)
                {
                    return Progresses[0];
                }
                return 0.0f;
            }
        }

        public float TotalProgress
        {
            get
            {
                if(Progresses!=null && Progresses.Length>0)
                {
                    return Progresses.Sum() / Progresses.Length;
                }
                return 0.0f;
            }
        }

        public SystemObject UserData { get; private set; }

        public bool IsDone { get; set; } = false;

        internal AssetHandler(string label,string[] addresses,SystemObject userData)
        {
            Label = label;
            Addresses = addresses;
            UserData = userData;

            Progresses = new float[addresses.Length];
            UObjects = new UnityObject[addresses.Length];
        }

        internal void DoCancel(bool isInstance,bool destroyIfIsInstnace)
        {
            if(isInstance && destroyIfIsInstnace)
            {
                for(int  i = 0;i<UObjects.Length;++i)
                {
                    UnityObject uObj = UObjects[i];
                    if(uObj!=null)
                    {
                        UnityObject.Destroy(uObj);
                        UObjects[i] = null;
                    }
                }
            }

            UObjects = null;
            Label = null;
            Addresses = null;
            Progresses = null;
            UserData = null;
        }
    }
}
