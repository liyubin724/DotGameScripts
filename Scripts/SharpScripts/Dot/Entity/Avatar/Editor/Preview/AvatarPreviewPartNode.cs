using Dot.Entity.Avatar;
using UnityEngine;
using XNode;
using System;
using Dot.XNodeEx;

namespace DotEditor.Entity.Avatar.Preview
{
    public class AvatarPreviewPartNode : DotNode {

        public AvatarPartType partType = AvatarPartType.None;
        [HideInInspector]
        public int selectedIndex = -1;
        [HideInInspector]
        [NonSerialized]
        public AvatarPartData[] parts = null;

        [Output]
        public AvatarPartData part = null;

        public override object GetValue(NodePort port) {
            if(port.fieldName == "part")
            {
                if(parts!=null && parts.Length>0 && selectedIndex>=0&&selectedIndex<parts.Length)
                {
                    return parts[selectedIndex];
                }
            }
		    return null; // Replace this
	    }
    }

}