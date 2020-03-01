using Dot.Log;
using Dot.Timer;
using Dot.Util;
using System.Collections.Generic;
using UnityEngine;

namespace Dot.Pool
{
    internal static class PoolConst
    {
        internal static readonly string LOGGER_NAME = "GameObjectPool";
    }

    public delegate void OnPoolComplete(string spawnName, string assetPath);

    public class PoolManager : Singleton<PoolManager>
    {
        private Transform mgrTransform = null;
        private Dictionary<string, SpawnPool> spawnDic = new Dictionary<string, SpawnPool>();

        private float cullTimeInterval = 60f;
        private TimerTaskInfo cullTimerTask = null;
        protected override void DoInit()
        {
            mgrTransform = DontDestroyHandler.CreateTransform("PoolManager");

            cullTimerTask = TimerManager.GetInstance().AddIntervalTimer(cullTimeInterval, OnCullTimerUpdate);

            LogUtil.LogInfo(PoolConst.LOGGER_NAME, "PoolManager::DoInit->PoolManager Start");
        }
        
        /// <summary>
        /// 判断是否存在指定的分组
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool HasSpawnPool(string name)=> spawnDic.ContainsKey(name);

        /// <summary>
        ///获取指定的分组，如果不存在可以指定isCreateIfNot为true进行创建
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isCreateIfNot"></param>
        /// <returns></returns>
        public SpawnPool GetSpawnPool(string name,bool isCreateIfNot = false)
        {
            if (spawnDic.TryGetValue(name, out SpawnPool pool))
            {
                return pool;
            }

            if(isCreateIfNot)
            {
                return CreateSpawnPool(name);
            }
            return null;
        }

        /// <summary>
        /// 创建指定名称的分组
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SpawnPool CreateSpawnPool(string name)
        {
            if (!spawnDic.TryGetValue(name, out SpawnPool pool))
            {
                pool = new SpawnPool(name, mgrTransform);
                spawnDic.Add(name, pool);
            }
            return pool;
        }

        /// <summary>
        /// 删除指定的分组，对应分组中所有的缓存池都将被删除
        /// </summary>
        /// <param name="name"></param>
        public void DeleteSpawnPool(string name)
        {
            if (spawnDic.TryGetValue(name, out SpawnPool spawn))
            {
                spawnDic.Remove(name);

                spawn.DestroySpawn();
            }
        }

        private void OnCullTimerUpdate(System.Object obj)
        {
            foreach(var kvp in spawnDic)
            {
                kvp.Value.CullSpawn(cullTimeInterval);
            }
        }
        
        public override void DoDispose()
        {
            foreach (var kvp in spawnDic)
            {
                kvp.Value.DestroySpawn();
            }
            spawnDic.Clear();

            if (cullTimerTask != null)
            {
                TimerManager.GetInstance().RemoveTimer(cullTimerTask);
            }
            cullTimerTask = null;
            spawnDic = null;
        }
       
    }
}
