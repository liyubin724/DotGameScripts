using UnityEngine;

namespace Dot.Entity.Controller
{
    public class EntityVirtualViewController : EntityControllerBase
    {
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
            GameObject gObj = new GameObject("Entity");
            entityBehaviour = gObj.AddComponent<EntityBehaviour>();
        }

        protected override void DoReset()
        {
            GameObject.Destroy(RootGameObject);

            rootGameObject = null;
            rootTransfrom = null;
            entityBehaviour = null;
        }
    }
}
