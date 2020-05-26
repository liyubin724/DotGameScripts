using Dot.Dispatch;
using UnityEngine;

namespace Dot.Entity.Controller
{
    public class EntityGameObjectController : EntityController
    {
        public GameObject RootGameObject { get; }

        public Transform RootTransform { get; }

        private string name = "";
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if(name !=value && !string.IsNullOrEmpty(value))
                {
                    name = value;
                    RootGameObject.name = name;
                }
            }
        }

        public Vector3 Position
        {
            get
            {
                return RootTransform.position;
            }
            set
            {
                RootTransform.position = value;
            }
        }

        public Vector3 Forward
        {
            get
            {
                return RootTransform.forward;
            }
            set
            {
                RootTransform.forward = value.normalized;
            }
        }

        public Vector3 Rotation
        {
            get
            {
                return RootTransform.rotation.eulerAngles;
            }
            set
            {
                RootTransform.rotation = Quaternion.Euler(value);
            }
        }

        public EntityGameObjectController(GameObject gameObject) : this(gameObject,"Entity")
        {            
        }

        public EntityGameObjectController(GameObject gameObject,string name)
        {
            RootGameObject = gameObject;
            RootTransform = RootGameObject.transform;
            Name = name;
        }

        protected override void OnInitialized()
        {
            
        }

        protected override void DoRegisterEvent()
        {
            RegisterEvent(EntityEventConst.ENTITY_POSITION_CHANGED, OnPositionChanged);
            RegisterEvent(EntityEventConst.ENTITY_ROTATION_CHANGED, OnRotationChanged);
            RegisterEvent(EntityEventConst.ENTITY_FORWARD_CHANGED, OnForwardChanged);
        }

        protected override void DoUnregisterEvent()
        {
            UnregisterEvent(EntityEventConst.ENTITY_POSITION_CHANGED, OnPositionChanged);
            UnregisterEvent(EntityEventConst.ENTITY_ROTATION_CHANGED, OnRotationChanged);
            UnregisterEvent(EntityEventConst.ENTITY_FORWARD_CHANGED, OnForwardChanged);
        }

        private void OnPositionChanged(EventData eventData)
        {
            Position = eventData.GetValue<Vector3>();
        }

        private void OnRotationChanged(EventData eventData)
        {
            Rotation = eventData.GetValue<Vector3>();
        }

        private void OnForwardChanged(EventData eventData)
        {
            Forward = eventData.GetValue<Vector3>();
        }
    }
}
