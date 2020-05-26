using Dot.Entity.Node;
using DotEditor.NativeDrawer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace DotEditor.Entity.Node
{
    [CustomEditor(typeof(NodeBehaviour))]
    public class NodeBehaviourEditor : Editor
    {
        private NativeDrawerObject drawerObject = null;

        void OnEnable()
        {
            drawerObject = new NativeDrawerObject(target)
            {
                IsShowScroll = true,
            };
        }

        public override void OnInspectorGUI()
        {
            drawerObject.OnGUILayout();
        }
    }
}
