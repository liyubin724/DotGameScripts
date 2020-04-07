using System;
using UnityEngine;

namespace Dot.GUI.Attributes
{
    [AttributeUsage(AttributeTargets.Field,AllowMultiple =false,Inherited =false)]
    public class TagSelectorAttribute : PropertyAttribute
    {
        public TagSelectorAttribute() { }
    }
}
