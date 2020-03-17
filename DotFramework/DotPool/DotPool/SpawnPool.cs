using Dot.Log;
using System.Collections.Generic;
using UnityEngine;

namespace Dot.Pool
{
    /// <summary>
    /// 主要用于GameObjectPool的分组，可以根据使用场景进行添加和删除分组，不同的分组中可以有相同GameObject的缓存池
    /// </summary>
    public class SpawnPool
    {
        internal string PoolName { get; private set; } = string.Empty;
        internal Transform SpawnTransform { get; private set; } = null;

        private Dictionary<string, GameObjectPool> gameObjectPools = new Dictionary<string, GameObjectPool>();

        internal SpawnPool(string pName, Transform parentTran)
        {
            PoolName = pName;

            SpawnTransform = new GameObject($"Spawn_{PoolName}").transform;
            SpawnTransform.SetParent(parentTran, false);
        }

        public bool HasGameObjectPool(string uniqueName)
        {
            return gameObjectPools.ContainsKey(uniqueName);
        }

        /// <summary>
        /// 缓存池中将默认以资源的路径为唯一标识，通过资源唯一标识可以获致到对应的缓存池
        /// </summary>
        /// <param name="uniqueName"></param>
        /// <returns></returns>
        public GameObjectPool GetGameObjectPool(string uniqueName)
        {
            if(gameObjectPools.TryGetValue(uniqueName,out GameObjectPool goPool))
            {
                return goPool;
            }

            return null;
        }

        /// <summary>
        /// 使用给定的GameObject创建缓存池
        /// </summary>
        /// <param name="uniqueName">资源唯一标签，一般使用资源路径</param>
        /// <param name="template">模板GameObject</param>
        /// <returns></returns>
        public GameObjectPool CreateGameObjectPool(string uniqueName,GameObject template, PoolTemplateType templateType = PoolTemplateType.Prefab)
        {
            if(template == null)
            {
                LogUtil.LogError(PoolConst.LOGGER_NAME, "SpawnPool::CreateGameObjectPool->Template is Null");
                return null;
            }

            if (gameObjectPools.TryGetValue(uniqueName, out GameObjectPool goPool))
            {
                LogUtil.LogWarning(PoolConst.LOGGER_NAME, "SpawnPool::CreateGameObjectPool->The pool has been created.uniqueName = " + uniqueName);
            }
            else
            {
                goPool = new GameObjectPool(this, uniqueName, template, templateType);

                gameObjectPools.Add(uniqueName, goPool);
            }

            return goPool;
        }
        /// <summary>
        /// 删除指定的缓存池
        /// </summary>
        /// <param name="uniqueName">资源唯一标签，一般使用资源路径</param>
        public void DeleteGameObjectPool(string uniqueName)
        {
            LogUtil.LogInfo(PoolConst.LOGGER_NAME, $"SpawnPool::DeleteGameObjectPool->Try to delete pool.uniqueName ={uniqueName}");
            
            GameObjectPool gObjPool = GetGameObjectPool(uniqueName);

            if(gObjPool!=null)
            {
                gObjPool.DestroyPool();
                gameObjectPools.Remove(uniqueName);
            }
        }

        internal void CullSpawn(float deltaTime)
        {
            foreach(var kvp in gameObjectPools)
            {
                if(kvp.Value.isCull)
                {
                    kvp.Value.CullPool(deltaTime);
                }
            }
        }

        internal void DestroySpawn()
        {
            foreach(var kvp in gameObjectPools)
            {
                kvp.Value.DestroyPool();
            }
            gameObjectPools.Clear();

            Object.Destroy(SpawnTransform.gameObject);
        }
    }
}
