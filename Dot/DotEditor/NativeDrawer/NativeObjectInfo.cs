using DotEditor.NativeDrawer.DefaultTypeDrawer;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DotEditor.NativeDrawer
{
    public class NativeObjectInfo : NativeInfo
    {
        private List<NativeInfo> fields = new List<NativeInfo>();
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
            
        }

        private void InitField()
        {
            
        }

        
    }
}
