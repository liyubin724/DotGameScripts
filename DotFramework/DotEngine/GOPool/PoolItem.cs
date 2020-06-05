using UnityEngine;

namespace DotEngine.GOPool
{
    public class PoolItem : MonoBehaviour
    {
        public string AssetPath { get; set; } = string.Empty;
        public string SpawnName { get; set; } = string.Empty;

        private Transform cachedTransform = null;
        public Transform CachedTransform
        {
            get {
                if (cachedTransform == null)
                {
                    cachedTransform = transform;
                }
                return cachedTransform;
            }
        }

        private GameObject cachedGameObject = null;
        public GameObject CachedGameObject
        {
            get
            {
                if(cachedGameObject == null)
                {
                    cachedGameObject = gameObject;
                }
                return cachedGameObject;
            }
        } 

        /// <summary>
        /// 
        /// </summary>
        public virtual void DoSpawned()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void DoDespawned()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public void ReleaseItem()
        {
            if (string.IsNullOrEmpty(AssetPath) || string.IsNullOrEmpty(SpawnName))
            {
                Destroy(CachedGameObject);
                return;
            }
            if (!PoolService.GetInstance().HasGroup(SpawnName))
            {
                Destroy(CachedGameObject);
                return;
            }
            PoolGroup spawnPool = PoolService.GetInstance().GetGroup(SpawnName);
            Pool gObjPool = spawnPool.GetPool(AssetPath);
            if (gObjPool == null)
            {
                Destroy(CachedGameObject);
                return;
            }
            gObjPool.ReleasePoolItem(CachedGameObject);
        }
    }
}
