﻿using System;

namespace Dot.GUI.Drawer.Condition
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class ShowIfAttribute : CompareAttribute
    {
        public ShowIfAttribute(string comparedName, object comparedValue) : base(comparedName, comparedValue)
        {
        }
    }
}
