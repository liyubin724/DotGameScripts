using UnityEngine;

namespace Dot.Entity.Controller
{
    public class EntityViewController : EntityController
    {
        private static readonly string GAMEOBJECT_REGISTER_NAME = "gameObject";
        private static readonly string TRANSFORM_REGISTER_NAME = "transform";

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

        public override EntityControllerType ControllerType
        {
            get => EntityControllerType.View;
        }

        public override string RegisterName
        {
            get => "viewController";
        }

        protected override void DoInit()
        {
            GameObject gObj = new GameObject($"Entity_{entityObj.Name}");
            entityBehaviour = gObj.AddComponent<EntityBehaviour>();

            rootTransfrom = entityBehaviour.transform;
            rootGameObject = entityBehaviour.gameObject;

            objTable.Set(EntityConst.GAMEOBJECT_REGISTER_NAME, rootGameObject);
            objTable.Set(EntityConst.TRANSFORM_REGISTER_NAME, rootTransfrom);
        }

        protected override void DoDestroy()
        {
            
        }
    }
}
