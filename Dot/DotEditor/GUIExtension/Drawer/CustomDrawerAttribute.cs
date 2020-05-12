﻿using System;

namespace DotEditor.GUIExtension.Drawer
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = false,Inherited = true)]
    public class CustomDrawerAttribute : Attribute
    {
        public Type AttrType { get; private set; }

        public CustomDrawerAttribute(Type attrType)
        {
            AttrType = attrType;
        }
    }
}