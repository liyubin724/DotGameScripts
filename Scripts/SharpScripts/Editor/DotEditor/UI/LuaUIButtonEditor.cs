using Dot.Lua.Event;
using Dot.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace DotEditor.UI
{
    [CustomEditor(typeof(LuaUIButton), true)]
    public class LuaUIButtonEditor : SelectableEditor
    {
        protected override void OnEnable()
        {
            base.OnEnable();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();


            if(GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}
