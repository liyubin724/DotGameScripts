using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace DotEditor.Entity.Avatar.Preview
{
    [CustomNodeEditor(typeof(AvatarPreviewNode))]
    public class AvatarPreviewNodeEditor : NodeEditor
    {
        public override void OnHeaderGUI()
        {
            base.OnHeaderGUI();  
        }

        public override void OnBodyGUI()
        {
            base.OnBodyGUI();
        }

        public override int GetWidth()
        {
            return 300;
        }
    }
}
