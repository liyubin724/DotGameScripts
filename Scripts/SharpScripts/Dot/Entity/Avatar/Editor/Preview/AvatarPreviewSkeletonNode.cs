using Dot.XNodeEx;
using System;
using UnityEngine;
using XNode;

namespace DotEditor.Entity.Avatar.Preview
{
    public class AvatarPreviewSkeletonNode : DotNode {

        [Output]
        public GameObject skeletonPrefab = null;

        [HideInInspector]
        public int selectedIndex = -1;

        [HideInInspector]
        [NonSerialized]
        public GameObject[] skeletons = null;

        public override object GetValue(NodePort port) 
        {
            if(port.fieldName == "skeletonPrefab")
            {
                if(skeletons!=null && skeletons.Length>0 && selectedIndex>=0&&selectedIndex<skeletons.Length)
                {
                    return skeletons[selectedIndex];
                }
            }
		    return null;
	    }
    }

}