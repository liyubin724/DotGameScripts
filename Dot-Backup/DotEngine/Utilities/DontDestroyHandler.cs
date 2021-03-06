﻿using UnityEngine;

namespace Dot.Utilities
{
    public static class DontDestroyHandler
    {
        private static readonly string RootName = "Singleton-Root";
        private static Transform rootTran = null;

        private static Transform RootTransform
        {
            get
            {
                if(rootTran == null)
                {
                    GameObject rootGO = GameObject.Find(RootName);
                    if (rootGO == null)
                    {
                        rootGO = new GameObject(RootName);
                    }
                    Object.DontDestroyOnLoad(rootGO);
                    rootTran = rootGO.transform;
                }

                return rootTran;
            }
        }

        public static Transform CreateTransform(string name)
        {
            GameObject behGO = new GameObject(name);
            Transform tran = behGO.transform;
            AddTransform(tran);
            return tran;
        }

        public static void AddTransform(Transform tran,bool worldPositionStays = false)
        {
            tran.SetParent(RootTransform, worldPositionStays);
        }

        public static T CreateComponent<T>(string name = null) where T : MonoBehaviour
        {
            T component = RootTransform.GetComponentInChildren<T>();
            if(component == null)
            {
                Transform tran = CreateTransform(string.IsNullOrEmpty(name)?typeof(T).Name:name);
                component = tran.gameObject.AddComponent<T>();
            }

            return component;
        }

        public static void Destroy()
        {
            if(rootTran!=null)
            {
                Object.Destroy(rootTran.gameObject);
                rootTran = null;
            }
        }
    }
}