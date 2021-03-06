﻿using DotEditor.XNodeEx;
using System.Linq;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace DotEditor.Entity.Avatar.Preview
{
    [CustomNodeEditor(typeof(AvatarPreviewSkeletonNode))]
    public class AvatarPreviewSkeletonNodeEditor : DotNodeEditor
    {
        private string[] names = null;

        AvatarPreviewSkeletonNode skeletonNode = null;
        public override void OnEnable()
        {
            skeletonNode = target as AvatarPreviewSkeletonNode;
            skeletonNode.skeletons = skeletonNode.GetGraph<AvatarPreviewGraph>().GetSkeletons();
            names = (from skeleton in skeletonNode.skeletons select skeleton.name).ToArray();
            if(names!=null && names.Length>0)
            {
                if(skeletonNode.selectedIndex<0 || skeletonNode.selectedIndex>=names.Length)
                {
                    skeletonNode.selectedIndex = 0;
                }
            }else
            {
                skeletonNode.selectedIndex = -1;
            }
        }

        public override void OnBodyGUI()
        {
            int newSelectedIndex = EditorGUILayout.Popup("Skeleton", skeletonNode.selectedIndex, names);
            if(newSelectedIndex!=skeletonNode.selectedIndex)
            {
                skeletonNode.selectedIndex = newSelectedIndex;
            }
            if(GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }

            serializedObject.Update();

            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("skeletonPrefab"));

            serializedObject.ApplyModifiedProperties();
        }

        public override int GetWidth()
        {
            return 300;
        }
    }
}
