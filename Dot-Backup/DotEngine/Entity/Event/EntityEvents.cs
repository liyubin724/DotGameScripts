using Dot.Entity.Avatar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Dot.Entity.Event
{
    public class BaseEntityEvent
    {
        public long ToUniqueID { get; set; }
    }

    public class EntityCreateEvent : BaseEntityEvent
    {
        public EntityObjectCategroy Categroy { get; set; }
        public bool IsBind { get; set; } = false;
        public long OwnerUniqueID { get; set; } = -1;
        public string BindNodeName { get; set; } 
    }

    public class EntityDeleteEvent :BaseEntityEvent
    {
    }

    public class EntityPositionChangedEvent : BaseEntityEvent
    {
        public Vector3 Position { get; set; }
    }

    public class EntityForwardChangedEvent : BaseEntityEvent
    {
        public Vector3 Forward { get; set; }
    }

    public class EntityRotationChangedEvent:BaseEntityEvent
    {
        public Vector3 Rotation { get; set; }
    }

    public class EntityAvatarSkeletonChangedEvent : BaseEntityEvent
    {
        public string Address{ get; set; }
    }

    public class EntityAssembleAvatarPartEvent : BaseEntityEvent
    {
        public AvatarPartType PartType { get; set; }
        public string PartAddress { get; set; }
    }

    public class EntityDisassembleAvatarPartEvent : BaseEntityEvent
    {
        public AvatarPartType PartType { get; set; }
    }
}
