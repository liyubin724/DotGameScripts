﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace DotEditor.NativeDrawer
{
    public class NativeObjectInfo : NativeInfo
    {
        private List<NativeValueInfo> fields = new List<NativeValueInfo>();
        private bool isFoldout = false;

        public NativeObjectInfo(object target):this(target,null)
        {
        }

        public NativeObjectInfo(object target,FieldInfo field) : base(target,field)
        {
            InitField();
        }

        public override void OnLayoutGUI()
        {
            throw new NotImplementedException();
        }

        private void InitField()
        {

        }

        
    }
}
