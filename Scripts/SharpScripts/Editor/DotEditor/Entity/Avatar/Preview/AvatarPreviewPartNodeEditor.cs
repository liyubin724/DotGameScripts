using DotEditor.XNodeEx;
using System.Linq;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace DotEditor.Entity.Avatar.Preview
{
    [CustomNodeEditor(typeof(AvatarPreviewPartNode))]
    public class AvatarPreviewPartNodeEditor : DotNodeEditor
    {
        AvatarPreviewPartNode partNode = null;
        private string[] partNames = null;
        public override void OnCreate()
        {
            base.OnCreate();
            partNode = target as AvatarPreviewPartNode;
            partNames = (from part in partNode.parts select part.name).ToArray();
            if (partNames != null && partNames.Length > 0)
            {
                if (partNode.selectedIndex < 0 || partNode.selectedIndex >= partNames.Length)
                {
                    partNode.selectedIndex = 0;
                }
            }
            else
            {
                partNode.selectedIndex = -1;
            }
        }

        public override int GetWidth()
        {
            return 300;
        }

        public override void OnBodyGUI()
        {
            base.OnBodyGUI();
            int newSelectedIndex = EditorGUILayout.Popup("Parts", partNode.selectedIndex, partNames);
            if (newSelectedIndex != partNode.selectedIndex)
            {
                partNode.selectedIndex = newSelectedIndex;
            }
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}
