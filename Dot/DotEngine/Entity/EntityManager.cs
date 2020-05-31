using Dot.Dispatch;
using Dot.Entity.Controller;
using Dot.Entity.Event;
using Dot.GOPool;
using Dot.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace Dot.Entity
{
    public class EntityManager : Singleton<EntityManager>
    {
        private readonly string ENTITY_ROOT_NAME = "Entity-Root";
        private readonly string ENTITY_POOL_NAME = "Entity-Pool";
        private readonly string ENTITY_ROOT_GO_TEMPLATE_NAME = "Entity-RootGO";

        private Transform rootTransform;
        private GameObjectPool rootGameObjectPool = null;

        private Dictionary<long, EntityObject> entityToIDDic = new Dictionary<long, EntityObject>();

        protected override void DoInit()
        {
            rootTransform = DontDestroyHandler.CreateTransform(ENTITY_ROOT_NAME);
            GameObjectPoolGroup poolGroup = GameObjectPoolManager.GetInstance().CreateGroup(ENTITY_POOL_NAME);

            GameObject template = new GameObject("entity-root_go");
            template.AddComponent<EntityObjectBehaviour>();

            rootGameObjectPool = poolGroup.CreatePool(ENTITY_ROOT_GO_TEMPLATE_NAME, template, PoolTemplateType.RuntimeInstance);
        }

        private void RegisterEvent()
        {
            EventManager eventManager = EventManager.GetInstance();
            eventManager.RegisterEvent(EntityEventConst.ENTITY_CREATE_EVENT, OnEntityCreate);
            eventManager.RegisterEvent(EntityEventConst.ENTITY_DELETE_EVENT, OnEntityDelete);

        }

        private void OnEntityCreate(EventData eventData)
        {

        }

        private void OnEntityDelete(EventData eventData)
        {

        }

        public EntityObject CreateEntity(long uniqueID)
        {
            EntityObject entity = new EntityObject();

            EntityGameObjectController goController = new EntityGameObjectController(rootGameObjectPool.GetPoolItem());
            entity.AddController(goController);

            entityToIDDic.Add(uniqueID, entity);

            return entity;
        }

        public EntityObject CreateAvatarEntity(long uniqueID)
        {
            EntityObject entity = CreateEntity(uniqueID);

            EntityAvatarController avatarController = new EntityAvatarController();
            entity.AddController(avatarController);

            return entity;
        }

    }
}
