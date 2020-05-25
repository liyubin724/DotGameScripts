using UnityEngine;

namespace Dot.Entity.Controller
{
    public class EntityGameObjectController : EntityController
    {
        private GameObject gameObject;
        private Transform transfrom;
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
                    gameObject.name = name;
                }
            }
        }

        public EntityGameObjectController(GameObject gameObject) : this(gameObject,"Entity")
        {            
        }

        public EntityGameObjectController(GameObject gameObject,string name)
        {
            this.gameObject = gameObject;
            transfrom = this.gameObject.transform;
            Name = name;
        }

        protected override void OnInitialized()
        {
            
        }

        public Vector3 Position
        {
            get
            {
                return transfrom.position;
            }
            set
            {
                transfrom.position = value;
            }
        }

        public Vector3 Forward
        {
            get
            {
                return transfrom.forward;
            }
            set
            {
                transfrom.forward = value.normalized;
            }
        }

        public Vector3 Rotation
        {
            get
            {
                return transfrom.rotation.eulerAngles;
            }
            set
            {
                transfrom.rotation = Quaternion.Euler(value);
            }
        }
    }
}
