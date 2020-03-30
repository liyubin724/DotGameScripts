using Dot.Asset.Datas;
using Dot.Log;
using Dot.Core;
using System;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;
using Dot.Core.Proxy;

namespace Dot.Asset
{
    /// <summary>
    /// 资源加载完成后回调
    /// </summary>
    /// <param name="address">资源地址</param>
    /// <param name="uObj">加载到的资源，如果为null表示加载失败</param>
    /// <param name="userData">自定义参数</param>
    public delegate void OnAssetLoadComplete(string address, UnityObject uObj, SystemObject userData);
    /// <summary>
    /// 资源加载进度回调，当资源加载进度变化时会回调
    /// </summary>
    /// <param name="address">资源地址</param>
    /// <param name="progress">当前资源加载进度</param>
    /// <param name="userData">自定义参数</param>
    public delegate void OnAssetLoadProgress(string address, float progress, SystemObject userData);

    /// <summary>
    /// 批量加载资源时回调
    /// </summary>
    /// <param name="addresses">一次加载所有资源的地址，地址可以重复</param>
    /// <param name="uObjs">加载完成后对应的资源，如果某个资源加载失败则为null</param>
    /// <param name="userData">自定义参数</param>
    public delegate void OnBatchAssetLoadComplete(string[] addresses, UnityObject[] uObjs, SystemObject userData);
    /// <summary>
    /// 批量加载资源进度的回调
    /// </summary>
    /// <param name="addresses">一次加载所有资源的地址，地址可以重复</param>
    /// <param name="progresses">对应资源加载进度</param>
    /// <param name="userData">自定义参数</param>
    public delegate void OnBatchAssetsLoadProgress(string[] addresses, float[] progresses, SystemObject userData);

    /// <summary>
    /// 资源加载模式
    /// </summary>
    public enum AssetLoaderMode
    {
        /// <summary>
        /// 使用AssetDatabase进行资源的加载，只适用于编辑器中
        /// </summary>
        AssetDatabase,
        /// <summary>
        /// 使用AB进行资源加载，可用于编辑器及运行时
        /// </summary>
        AssetBundle,
    }

    /// <summary>
    /// 资源加载的优先级，优先级越高，加载时会优先加载。
    /// 同等优先级下按资源的加载顺序加载
    /// </summary>
    public enum AssetLoaderPriority
    {
        VeryLow = 100,
        Low = 200,
        Default = 300,
        High = 400,
        VeryHigh = 500,
    }

    /// <summary>
    /// 单例模式的资源加载管理
    /// </summary>
    public partial class AssetManager : Singleton<AssetManager>
    {
        private AAssetLoader assetLoader = null;
        private ASceneLoader sceneLoader = null;

        private AssetLoaderMode loaderMode = AssetLoaderMode.AssetDatabase;

        /// <summary>
        /// 初始化资源加载器。
        /// 在使用前必须先进行初始化，初始成功后才能正常使用
        /// </summary>
        /// <param name="mode">资源加载模式,参见<see cref="AssetLoaderMode"/></param>
        /// <param name="initCallback">管理器初始化成功后回调，成功返回true,否则返回false</param>
        /// <param name="assetRootDir">使用AB加载资源时，需要指定AB资源所在的根目录</param>
        public void InitManager(AssetLoaderMode mode,
            Action<bool> initCallback,
            string assetRootDir = "")
        {
            loaderMode = mode;

            LogUtil.LogInfo(AssetConst.LOGGER_NAME, $"AssetManager::InitManager->Start init mgr.mode = {mode.ToString()},assetRootDir = {assetRootDir}");

            if(loaderMode == AssetLoaderMode.AssetBundle)
            {
                assetLoader = new BundleLoader();
            }
#if UNITY_EDITOR
            else
            {
                assetLoader = new DatabaseLoader();
            }
#endif

            if(assetLoader == null)
            {
                LogUtil.LogError(AssetConst.LOGGER_NAME, "AssetLoader is Null");
                initCallback?.Invoke(false);
            }else
            {
                assetLoader.Initialize((Action<bool>)((result) =>
                {
                    if(!result)
                    {
                        LogUtil.LogError((string)AssetConst.LOGGER_NAME, "AssetManager::InitManager->init failed");
                    }

                    LogUtil.LogInfo((string)AssetConst.LOGGER_NAME, "AssetManager::InitManager->init Success");

                    if (loaderMode == AssetLoaderMode.AssetBundle)
                    {
                        sceneLoader = new BundleSceneLoader(assetLoader);
                    }
#if UNITY_EDITOR
                    else
                    {
                        sceneLoader = new DatabaseSceneLoader(assetLoader);
                    }
#endif
                    if(result)
                    {
                        StartAutoClean();
                    }

                    initCallback.Invoke(result);
                }),  assetRootDir);
            }
        }

        /// <summary>
        /// 修改可同时进行加载的加载器的数量
        /// 注意:此数量并非指定资源的数量
        /// </summary>
        /// <param name="count">加载器的最大数量</param>
        public void ChangeMaxLoadingCount(int count)
        {
            if(assetLoader!=null)
            {
                LogUtil.LogInfo(AssetConst.LOGGER_NAME, $"AssetManager::ChangeMaxLoadingCount->Change Count from {assetLoader.MaxLoadingCount} to {count}");
                assetLoader.MaxLoadingCount = count;
            }else
            {
                LogUtil.LogError(AssetConst.LOGGER_NAME, "AssetManager::ChangeMaxLoadingCount->assetloader is null");
            }
        }

        /// <summary>
        /// 异步加载设定为指定标签的所有资源
        /// </summary>
        /// <param name="label">资源的标签</param>
        /// <param name="complete">单个资源加载完成时回调</param>
        /// <param name="batchComplete">所有资源加载完毕时回调</param>
        /// <param name="progress">单个资源加载进度回调</param>
        /// <param name="batchProgress">所有资源加载进度回调</param>
        /// <param name="priority">优先级</param>
        /// <param name="userData">自定义参数</param>
        /// <returns></returns>
        public AssetHandler LoadAssetAsyncByLabel(string label,
            OnAssetLoadComplete complete,
            OnBatchAssetLoadComplete batchComplete,
            OnAssetLoadProgress progress = null,
            OnBatchAssetsLoadProgress batchProgress = null,
            AssetLoaderPriority priority = AssetLoaderPriority.Default,
            SystemObject userData = null)
        {
            return LoadBatchAssetAsync(label,null,false, complete, batchComplete, progress, batchProgress, priority, userData);
        }

        /// <summary>
        /// 异步加载指定资源
        /// </summary>
        /// <param name="assetPath">资源地址</param>
        /// <param name="complete">资源加载完毕后回调</param>
        /// <param name="progress">资源加载进度的回调</param>
        /// <param name="priority">优先级</param>
        /// <param name="userData">自定义参数</param>
        /// <returns></returns>
        public AssetHandler LoadAssetAsync(string assetPath,
            OnAssetLoadComplete complete,
            OnAssetLoadProgress progress = null,
            AssetLoaderPriority priority = AssetLoaderPriority.Default,
            SystemObject userData = null)
        {
            return LoadBatchAssetAsync(null, new string[] { assetPath }, false, complete, null, progress, null, priority, userData);
        }

        /// <summary>
        /// 异步进行批量资源加载
        /// </summary>
        /// <param name="addresses">所有资源的地址</param>
        /// <param name="complete">单个资源加载完毕后回调</param>
        /// <param name="batchComplete">所有资源加载完毕后回调</param>
        /// <param name="progress">单个资源加载进度回调</param>
        /// <param name="batchProgress">所有资源加载进度回调</param>
        /// <param name="priority">优先级</param>
        /// <param name="userData">自定义参数</param>
        /// <returns></returns>
        public AssetHandler LoadAssetAsync(string[] addresses, 
            OnAssetLoadComplete complete,
            OnBatchAssetLoadComplete batchComplete,
            OnAssetLoadProgress progress = null,
            OnBatchAssetsLoadProgress batchProgress = null,
            AssetLoaderPriority priority = AssetLoaderPriority.Default,
            SystemObject userData = null)
        {
            return LoadBatchAssetAsync(null, addresses,false, complete, batchComplete, progress, batchProgress, priority, userData);
        }

        /// <summary>
        /// 异步加载并实例化设定为指定标签的所有资源
        /// 多用于Prefab的加载，资源加载完成后会将其实例化
        /// </summary>
        /// <param name="label">资源的标签</param>
        /// <param name="complete">单个资源加载完成时回调</param>
        /// <param name="batchComplete">所有资源加载完毕时回调</param>
        /// <param name="progress">单个资源加载进度回调</param>
        /// <param name="batchProgress">所有资源加载进度回调</param>
        /// <param name="priority">优先级</param>
        /// <param name="userData">自定义参数</param>
        /// <returns></returns>
        public AssetHandler InstanceAssetAsync(string label,
            OnAssetLoadComplete complete,
            OnBatchAssetLoadComplete batchComplete,
            OnAssetLoadProgress progress = null,
            OnBatchAssetsLoadProgress batchProgress = null,
            AssetLoaderPriority priority = AssetLoaderPriority.Default,
            SystemObject userData = null)
        {
            return LoadBatchAssetAsync(label, null, true, complete, batchComplete, progress, batchProgress, priority, userData);
        }

        /// <summary>
        /// 异步加载并实例化指定资源
        /// </summary>
        /// <param name="assetPath">资源地址</param>
        /// <param name="complete">资源加载完毕后回调</param>
        /// <param name="progress">资源加载进度的回调</param>
        /// <param name="priority">优先级</param>
        /// <param name="userData">自定义参数</param>
        /// <returns></returns>
        public AssetHandler InstanceAssetAsync(string assetPath,
            OnAssetLoadComplete complete,
            OnAssetLoadProgress progress = null,
            AssetLoaderPriority priority = AssetLoaderPriority.Default,
            SystemObject userData = null)
        {
            return LoadBatchAssetAsync(null, new string[] { assetPath }, true, complete, null, progress, null, priority, userData);
        }

        /// <summary>
        /// 异步进行批量资源加载并实例化
        /// </summary>
        /// <param name="addresses">所有资源的地址</param>
        /// <param name="complete">单个资源加载完毕后回调</param>
        /// <param name="batchComplete">所有资源加载完毕后回调</param>
        /// <param name="progress">单个资源加载进度回调</param>
        /// <param name="batchProgress">所有资源加载进度回调</param>
        /// <param name="priority">优先级</param>
        /// <param name="userData">自定义参数</param>
        /// <returns></returns>
        public AssetHandler InstanceAssetAsync(string[] addresses,
            OnAssetLoadComplete complete,
            OnBatchAssetLoadComplete batchComplete,
            OnAssetLoadProgress progress = null,
            OnBatchAssetsLoadProgress batchProgress = null,
            AssetLoaderPriority priority = AssetLoaderPriority.Default,
            SystemObject userData = null)
        {
            return LoadBatchAssetAsync(null, addresses, true, complete, batchComplete, progress, batchProgress, priority, userData);
        }

        private AssetHandler LoadBatchAssetAsync(
            string label, 
            string[] addresses,
            bool isInstance,
            OnAssetLoadComplete complete,
            OnBatchAssetLoadComplete batchComplete,
            OnAssetLoadProgress progress,
            OnBatchAssetsLoadProgress batchProgress,
            AssetLoaderPriority priority,
            SystemObject userData)
        {
            if(assetLoader == null)
            {
                LogUtil.LogError(AssetConst.LOGGER_NAME, "AssetManager::LoadBatchAssetAsync->assetLoader is Null");
                return null;
            }

            return assetLoader.LoadBatchAssetAsync(label, addresses, isInstance, complete, batchComplete, progress, batchProgress, priority, userData);
        }

        /// <summary>
        /// 停止正在加载中的资源
        /// 如果资源已经加载完毕，调用后没有任何效果，只需要使用Destroy销毁资源即可。
        /// 如果资源在加载中，则会终止资源的回调（对于使用AB加载的话，由于Unity底层接口无法停止的问题，
        /// 所以并不会真正停止，只是不再回调加载完成后的接口），如果此资源需要实例化，则会根据<paramref name="destroyIfIsInstnace"/>
        /// 来判断是否删除已经实例化的资源
        /// </summary>
        /// <param name="handler">调用加载接口后返回的Handler</param>
        /// <param name="destroyIfIsInstnace">如果资源需要实例化时，对于已经实例化的实例是否需要删除，如果指定为true，则会调用Destroy销毁</param>
        public void UnloadAssetAsync(AssetHandler handler,bool destroyIfIsInstnace = false)
        {
            if (assetLoader == null)
            {
                LogUtil.LogError(AssetConst.LOGGER_NAME, "AssetManager::UnloadAssetAsync->assetLoader is Null");
                return;
            }

            assetLoader.UnloadAssetAsync(handler, destroyIfIsInstnace);
        }

        /// <summary>
        /// 使用资源加载接口只加载资源后，如果需要实例化时，需要统一通过此接口
        /// 注意：必须使用此接口才能保证资源正常的实例化
        /// </summary>
        /// <param name="address">资源地址</param>
        /// <param name="asset">加载到的资源</param>
        /// <returns></returns>
        public UnityObject InstantiateAsset(string address, UnityObject asset)
        {
            if (assetLoader == null)
            {
                LogUtil.LogError(AssetConst.LOGGER_NAME, "AssetManager::InstantiateAsset->assetLoader is Null");
                return null;
            }

            return assetLoader.InstantiateAsset(address, asset);
        }

        protected override void DoInit()
        {
            UpdateProxy.GetInstance().DoUpdateHandle += DoUpdate;
        }

        private void DoUpdate(float deltaTime)
        {
            assetLoader?.DoUpdate(deltaTime);
            sceneLoader?.DoUpdate(deltaTime);
        }

        public override void DoDispose()
        {
            UpdateProxy.GetInstance().DoUpdateHandle -= DoUpdate;
            DoDispose_Clean();

            assetLoader?.DoDispose();
            base.DoDispose();
        }
    }
}
