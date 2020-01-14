﻿using Dot.Timer;
using Dot.Core.Util;
using Dot.Log;
using System;
using System.Collections.Generic;
using UnityEngine;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;
using Dot.Asset;
using Dot.Util;

namespace Dot.Pool
{
    /// <summary>
    /// 用于创建缓存池的模板的类型
    /// </summary>
    public enum PoolTemplateType
    {
        Prefab,//使用Prefab做为缓存池模型
        PrefabInstance,//使用Prefab实例化后的对象做为模板
        RuntimeInstance,//运行时创建的对象做模板
    }

    public class GameObjectPool
    {
        private SpawnPool spawnPool = null;

        private string uniqueName = null;
        private PoolTemplateType templateType = PoolTemplateType.Prefab;
        private GameObject instanceOrPrefabTemplate = null;

        internal GameObject Template { get => instanceOrPrefabTemplate; }

        private Queue<GameObject> unusedItemQueue = new Queue<GameObject>();
        private List<WeakReference<GameObject>> usedItemList = new List<WeakReference<GameObject>>();

        public OnPoolComplete completeCallback = null;

        public int preloadTotalAmount = 0;
        public int preloadOnceAmount = 1;

        public bool isCull = false;
        public int cullOnceAmount = 0;
        public int cullDelayTime = 30;

        public int limitMaxAmount = 0;
        public int limitMinAmount = 0;

        private float preCullTime = 0;
        private float curTime = 0;

        private TimerTaskInfo preloadTimerTask = null;

        internal GameObjectPool(SpawnPool pool, string aPath, GameObject templateGObj, PoolTemplateType templateType)
        {
            spawnPool = pool;
            uniqueName = aPath;

            instanceOrPrefabTemplate = templateGObj;
            this.templateType = templateType;

            if(templateType!= PoolTemplateType.Prefab)
            {
                instanceOrPrefabTemplate.SetActive(false);
                instanceOrPrefabTemplate.transform.SetParent(pool.SpawnTransform, false);
            }

            preloadTimerTask = TimerManager.GetInstance().AddIntervalTimer(0.05f, OnPreloadTimerUpdate);
        }

        #region Preload
        /// <summary>
        /// 使用Timer的Tick进行预加载
        /// </summary>
        /// <param name="obj"></param>
        private void OnPreloadTimerUpdate(SystemObject obj)
        {
            int curAmount = usedItemList.Count + unusedItemQueue.Count;
            if (curAmount >= preloadTotalAmount)
            {
                OnPoolComplete();
            }
            else
            {
                int poa = preloadOnceAmount;
                if (poa == 0)
                {
                    poa = preloadTotalAmount;
                }
                else
                {
                    poa = Mathf.Min(preloadOnceAmount, preloadTotalAmount - curAmount);
                }
                for (int i = 0; i < poa; ++i)
                {
                    GameObject instance = CreateNewItem();
                    instance.transform.SetParent(spawnPool.SpawnTransform, false);
                    instance.SetActive(false);
                    unusedItemQueue.Enqueue(instance);
                }
            }
        }

        private void OnPoolComplete()
        {
            completeCallback?.Invoke(spawnPool.PoolName, uniqueName);
            completeCallback = null;

            if(preloadTimerTask!=null)
            {
                TimerManager.GetInstance().RemoveTimer(preloadTimerTask);
                preloadTimerTask = null;
            }
        }

        #endregion

        #region GetItem
        /// <summary>
        /// 从缓存池中得到一个GameObject对象
        /// </summary>
        /// <param name="isAutoSetActive">是否激获取到的GameObject,默认为true</param>
        /// <returns></returns>
        public GameObject GetPoolItem(bool isAutoSetActive = true)
        {
            if (limitMaxAmount != 0 && usedItemList.Count > limitMaxAmount)
            {
                LogUtil.LogWarning(typeof(GameObjectPool), "GameObjectPool::GetItem->Large than Max Amount");
                return null;
            }

            GameObject item = null;
            if (unusedItemQueue.Count > 0)
            {
                item = unusedItemQueue.Dequeue();
            }
            else
            {
                item = CreateNewItem();
            }

            if (item != null)
            {
                GameObjectPoolItem poolItem = item.GetComponent<GameObjectPoolItem>();
                if (poolItem != null)
                {
                    poolItem.DoSpawned();
                }
            }

            if (isAutoSetActive)
            {
                item.SetActive(true);
            }

            usedItemList.Add(new WeakReference<GameObject>(item));
            return item;
        }

        /// <summary>
        /// 从缓存池中得到指定类型的组件
        /// </summary>
        /// <typeparam name="T">继承于MonoBehaviour的组件</typeparam>
        /// <param name="isAutoActive">是否激获取到的GameObject,默认为true</param>
        /// <returns></returns>
        public T GetComponentItem<T>(bool isAutoActive = true,bool isAddIfNotFind = false) where T:MonoBehaviour
        {
            if(instanceOrPrefabTemplate.GetComponent<T> ()==null && !isAddIfNotFind)
            {
                return null;
            }

            GameObject gObj = GetPoolItem(isAutoActive);
            T component = null;
            if(gObj!=null)
            {
                component = gObj.GetComponent<T>();
                if(component == null)
                {
                    component = gObj.AddComponent<T>();
                    GameObjectPoolItem poolItem = component as GameObjectPoolItem;
                    if(poolItem!=null)
                    {
                        poolItem.SpawnName = spawnPool.PoolName;
                        poolItem.AssetPath = uniqueName;
                        poolItem.DoSpawned();
                    }
                }
            }

            return component;
        }

        private GameObject CreateNewItem()
        {
            GameObject item = null;
            if(templateType == PoolTemplateType.RuntimeInstance)
            {
                item = GameObject.Instantiate(instanceOrPrefabTemplate);
            }
            else
            {
#if ASSET_BUNDLE
                item = (GameObject)AssetManager.GetInstance().InstantiateAsset(uniqueName, instanceOrPrefabTemplate);
#else
                item = GameObject.Instantiate(instanceOrPrefabTemplate);
#endif
            }

            if (item != null)
            {
                GameObjectPoolItem poolItem = item.GetComponent<GameObjectPoolItem>();
                if (poolItem != null)
                {
                    poolItem.AssetPath = uniqueName;
                    poolItem.SpawnName = spawnPool.PoolName;
                }
            }
            return item;
        }
#endregion

#region Release Item
        /// <summary>
        /// 回收GameObject，如果此GameObject不带有GameObjectPoolItem组件，则无法回收到池中，将会直接删除
        /// </summary>
        /// <param name="item"></param>
        public void ReleasePoolItem(GameObject item)
        {
            if(item == null)
            {
                LogUtil.LogError(typeof(GameObjectPool), "GameObjectPool::ReleaseItem->Item is Null");
                return;
            }

            GameObjectPoolItem pItem = item.GetComponent<GameObjectPoolItem>();
            if(pItem!=null)
            {
                pItem.DoDespawned();
            }

            item.transform.SetParent(spawnPool.SpawnTransform, false);
            item.SetActive(false);
            unusedItemQueue.Enqueue(item);

            for (int i = usedItemList.Count - 1; i >= 0; i--)
            {
                if(usedItemList[i].TryGetTarget(out GameObject target))
                {
                    if(!target.IsNull())
                    {
                        if(target!=item)
                        {
                            continue;
                        }else
                        {
                            usedItemList.RemoveAt(i);
                            break;
                        }
                    }
                }

                usedItemList.RemoveAt(i);
            }
        }
#endregion


        internal void CullPool(float deltaTime)
        {
            for (int i = usedItemList.Count - 1; i >= 0; i--)
            {
                if (usedItemList[i].TryGetTarget(out GameObject target))
                {
                    if (!target.IsNull())
                    {
                        continue;
                    }
                }
                usedItemList.RemoveAt(i);
            }

            if (!isCull)
            {
                return;
            }

            curTime += deltaTime;
            if(curTime - preCullTime < cullDelayTime)
            {
                return;
            }

            int cullAmout = 0;
            if (usedItemList.Count + unusedItemQueue.Count <= limitMinAmount)
            {
                cullAmout = 0;
            }
            else
            {
                cullAmout = usedItemList.Count + unusedItemQueue.Count - limitMinAmount;
                if (cullAmout > unusedItemQueue.Count)
                {
                    cullAmout = unusedItemQueue.Count;
                }
            }

            if (cullOnceAmount > 0 && cullOnceAmount < cullAmout)
            {
                cullAmout = cullOnceAmount;
            }

            for (int i = 0; i < cullAmout && unusedItemQueue.Count>0; i++)
            {
                UnityObject.Destroy(unusedItemQueue.Dequeue());
            }

            preCullTime = curTime;
        }
        
        internal void DestroyPool()
        {
            completeCallback = null;
            if (preloadTimerTask != null)
            {
                TimerManager.GetInstance().RemoveTimer(preloadTimerTask);
                preloadTimerTask = null;
            }

            usedItemList.Clear();

            for (int i = unusedItemQueue.Count - 1; i >= 0; i--)
            {
                UnityObject.Destroy(unusedItemQueue.Dequeue());
            }
            unusedItemQueue.Clear();

            if(templateType == PoolTemplateType.PrefabInstance || templateType == PoolTemplateType.RuntimeInstance)
            {
                UnityObject.Destroy(instanceOrPrefabTemplate);
            }

            uniqueName = null;
            spawnPool = null;
            instanceOrPrefabTemplate = null;
        }
    }
}
