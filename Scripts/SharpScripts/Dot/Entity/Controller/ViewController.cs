using UnityEngine;

namespace Dot.Entity.Controller
{
    public class ViewController : EntityController
    {
        private static readonly string GAMEOBJECT_REGISTER_NAME = "csGameObject";
        private static readonly string TRANSFORM_REGISTER_NAME = "csTransform";

        private EntityBehaviour entityBehaviour = null;
        private Transform rootTransfrom = null;
        public Transform RootTransfrom
        {
            get => rootTransfrom;
        }

        public GameObject rootGameObject = null;
        public GameObject RootGameObject
        {
            get => rootGameObject;
        }

        protected override void DoInit()
        {
            GameObject gObj = new GameObject($"Entity_{entityObj.Name}");
            entityBehaviour = gObj.AddComponent<EntityBehaviour>();

            rootTransfrom = entityBehaviour.transform;
            rootGameObject = entityBehaviour.gameObject;

            objTable.Set(GAMEOBJECT_REGISTER_NAME, rootGameObject);
            objTable.Set(TRANSFORM_REGISTER_NAME, rootTransfrom);
        }

        protected override void DoDestroy()
        {
            
        }

        protected override void DoReset()
        {
        }
    }
}
