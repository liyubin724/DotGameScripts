﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEditor.NativeDrawer
{
    public class NativeDrawerObject
    {
        public object DrawerObject { get; private set; }

        public NativeDrawerObject(object obj)
        {
            DrawerObject = obj;
        }


    }
}
