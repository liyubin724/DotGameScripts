using System;
using UnityEngine;

namespace Dot.GUI.Attributes
{
    [AttributeUsage(AttributeTargets.Field,AllowMultiple =false,Inherited =false)]
    public class SuffixAttribute : PropertyAttribute
    {
        public string SuffixLabel { get; set; } = string.Empty;
        public string IconPath { get; set; } = string.Empty;
        public SuffixAttribute(string label,string iconPath = null)
        {
            SuffixLabel = label;
            IconPath = iconPath;
        }
    }
}
