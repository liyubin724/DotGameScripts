using UnityEngine;

namespace Dot.Entity.Controller
{
    public class EntityVirtualViewController : EntityController
    {
        private static readonly string GAMEOBJECT_REGISTER_NAME = "gameObject";
        private static readonly string TRANSFORM_REGISTER_NAME = "transform";

        private EntityBehaviour entityBehaviour = null;
        private Transform rootTransfrom = null;
        public Transform RootTransfrom
        {
            get
            {
                if(rootTransfrom == null)
                {
                    rootTransfrom = entityBehaviour.transform;
                }
                return rootTransfrom;
            }
        }

        public GameObject rootGameObject = null;
        public GameObject RootGameObject
        {
            get
            {
                if(rootGameObject == null)
                {
                    rootGameObject = entityBehaviour.gameObject;
                }
                return rootGameObject;
            }
        }

        protected override void DoInit()
        {
            GameObject gObj = new GameObject("Entity Virtual View");
            entityBehaviour = gObj.AddComponent<EntityBehaviour>();

            rootTransfrom = entityBehaviour.transform;
            rootGameObject = entityBehaviour.gameObject;

            objTable.Set(GAMEOBJECT_REGISTER_NAME, rootGameObject);
            objTable.Set(TRANSFORM_REGISTER_NAME, rootTransfrom);
        }

        protected override void DoReset()
        {
            rootGameObject = null;
            rootTransfrom = null;
            entityBehaviour = null;
            GameObject.Destroy(RootGameObject);
        }
    }
}
