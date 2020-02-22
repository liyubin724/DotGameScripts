using System;
using UnityEngine;
using XNode;

namespace DotEditor.Entity.Avatar.Preview
{
    public class AvatarPreviewSkeletonNode : XNode.Node {

        [Output]
        public GameObject skeletonPrefab = null;

        [HideInInspector]
        public int selectedIndex = -1;

        [HideInInspector]
        [NonSerialized]
        public GameObject[] skeletons = null;

	    protected override void Init() {
		    base.Init();
            skeletons = (graph as AvatarPreviewGraph).GetSkeletons();
	    }

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