﻿using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer
{
    public class NativeDrawerObject
    {
        public bool IsShowScroll { get; set; } = false;

        private object drawerObject;
        private List<NativeDrawerProperty> drawerProperties = new List<NativeDrawerProperty>();

        public NativeDrawerObject(object obj)
        {
            drawerObject = obj;

            InitField();
        }

        private void InitField()
        {
            Type[] allTypes = NativeDrawerUtility.GetAllBaseTypes(drawerObject.GetType());
            if(allTypes!=null)
            {
                foreach (var type in allTypes)
                {
                    FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    foreach (var field in fields)
                    {
                        NativeDrawerProperty drawerProperty = new NativeDrawerProperty(drawerObject, field);
                        drawerProperties.Add(drawerProperty);
                    }
                }
            }
        }

        private Vector2 scrollPos = Vector2.zero;

        public void OnGUILayout()
        {
            if(IsShowScroll)
            {
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos,GUILayout.ExpandHeight(false));
            }

            foreach(var property in drawerProperties)
            {
                property.OnGUILayout();
            }
            
            if(IsShowScroll)
            {
                EditorGUILayout.EndScrollView();
            }

        }
    }
}
