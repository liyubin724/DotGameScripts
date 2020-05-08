using Dot.Core;
using Dot.Core.Util;
using Dot.Core.Log;
using Dot.Timer;
using System.Collections.Generic;
using UnityEngine;

namespace Dot.Pool
{
    public delegate void PoolPreloadComplete(string groupName, string assetPath);

    public class GameObjectPoolManager : Singleton<GameObjectPoolManager>
    {
        private Transform mgrTransform = null;
        private Dictionary<string, GameObjectPoolGroup> groupDic = new Dictionary<string, GameObjectPoolGroup>();

        private float cullTimeInterval = 60f;
        private TimerTaskInfo cullTimerTask = null;
        protected override void DoInit()
        {
            LogUtil.LogInfo(GameObjectPoolConst.LOGGER_NAME, "PoolManager::DoInit->PoolManager Start");
            mgrTransform = DontDestroyHandler.CreateTransform(GameObjectPoolConst.MANAGER_NAME);

            cullTimerTask = TimerManager.GetInstance().AddIntervalTimer(cullTimeInterval, OnCullTimerUpdate);
        }
        
        /// <summary>
        /// 判断是否存在指定的分组
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool HasGroup(string name)=> groupDic.ContainsKey(name);

        /// <summary>
        ///获取指定的分组，如果不存在可以指定isCreateIfNot为true进行创建
        /// </summary>
        /// <param name="name"></param>
        /// <param name="autoCreateIfNot"></param>
        /// <returns></returns>
        public GameObjectPoolGroup GetGroup(string name,bool autoCreateIfNot = false)
        {
            if (groupDic.TryGetValue(name, out GameObjectPoolGroup pool))
            {
                return pool;
            }

            if(autoCreateIfNot)
            {
                return CreateGroup(name);
            }
            return null;
        }

        /// <summary>
        /// 创建指定名称的分组
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GameObjectPoolGroup CreateGroup(string name)
        {
            if (!groupDic.TryGetValue(name, out GameObjectPoolGroup pool))
            {
                pool = new GameObjectPoolGroup(name, mgrTransform);
                groupDic.Add(name, pool);
            }
            return pool;
        }

        /// <summary>
        /// 删除指定的分组，对应分组中所有的缓存池都将被删除
        /// </summary>
        /// <param name="name"></param>
        public void DeleteGroup(string name)
        {
            if (groupDic.TryGetValue(name, out GameObjectPoolGroup spawn))
            {
                groupDic.Remove(name);
                spawn.DestroyGroup();
            }
        }

        private void OnCullTimerUpdate(System.Object obj)
        {
            foreach(var kvp in groupDic)
            {
                kvp.Value.CullGroup(cullTimeInterval);
            }
        }
        
        public override void DoDispose()
        {
            foreach (var kvp in groupDic)
            {
                kvp.Value.DestroyGroup();
            }
            groupDic.Clear();

            if (cullTimerTask != null)
            {
                TimerManager.GetInstance().RemoveTimer(cullTimerTask);
            }
            cullTimerTask = null;
            groupDic = null;
        }
    }
}
