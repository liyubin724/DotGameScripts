using Dot.Asset;
using Dot.Entity.Avatar;
using Dot.Entity.Node;
using Dot.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XLua;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace Dot.Entity.Controller
{
    public class EntityAvatarController : EntityController
    {
        private static readonly string SKELETON_LOAD_NAME = "DoLoadSkeletonComplete";
        private static readonly string SKELETON_LOAD_CANCEL_NAME = "DoLoadSkeletonCancel";
        private static readonly string SKELETON_UNLOAD_NAME = "DoUnloadSkeletonComplete";

        private static readonly string PART_LOAD_NAME = "DoLoadPartComplete";
        private static readonly string PART_LOAD_CANCEL_NAME = "DoLoadPartCancel";
        private static readonly string PART_UNLOAD_NAME = "DoUnloadPartComplete";

        private static readonly string SKELETON_GAMEOBJECT_REGISTER_NAME = "skeletonGameObject";
        private static readonly string SKELETON_TRANSFORM_REGISTER_NAME = "skeletonTransfrom";

        private Transform RootTransform
        {
            get
            {
                ViewController vvc = entityObj.GetController<ViewController>(EntityControllerType.View);
                return vvc.RootTransfrom;
            }
        }

        private AssetBridge assetBridge = null;

        private GameObject skeletonGameObject = null;
        private Transform skeletonTransform = null;
        private NodeBehaviour nodeBehaviour = null;

        private long skeletonLoadingID = -1;

        private Dictionary<AvatarPartType, AvatarPartInstance> partInstanceDic = new Dictionary<AvatarPartType, AvatarPartInstance>();
        private Dictionary<AvatarPartType, long> partLoadingDic = new Dictionary<AvatarPartType, long>();

        protected override void DoInit()
        {
            assetBridge = new AssetBridge();
        }

        public NodeData GetBindNodeData(string nodeName)
        {
            if(nodeBehaviour == null)
            {
                LogUtil.LogError(GetType(), "EntityAvatarController::GetBindNode->nodeBehaviour is not been loaded");
                return null;
            }

            return nodeBehaviour.GetNode(NodeType.BindNode, nodeName);
        }

        public Transform BindTransfrom(string nodeName,Transform transform,
            Vector3 positionOffset,Vector3 rotationOffset)
        {
            NodeData bindNodeData = GetBindNodeData(nodeName);
            transform.SetParent(bindNodeData.transform, false);
            transform.localPosition = positionOffset;
            transform.localRotation = Quaternion.Euler(rotationOffset);

            return bindNodeData.transform;
        }

        public void LoadSkeleton(string skeletonAddress)
        {
            if(skeletonLoadingID>=0)
            {
                CancelLoadSkeleton();
            }
            skeletonLoadingID = assetBridge.InstanceAsset(skeletonAddress, OnSkeletonComplete);
        }

        private void OnSkeletonComplete(string address,UnityObject uObj,SystemObject userData)
        {
            if(skeletonGameObject !=null)
            {
                UnloadSkeleton();
            }
            skeletonLoadingID = -1;

            skeletonGameObject = uObj as GameObject;
            skeletonTransform = skeletonGameObject.transform;
            nodeBehaviour = skeletonGameObject.GetComponent<NodeBehaviour>();

            if(nodeBehaviour == null)
            {
                LogUtil.LogError(GetType(), "EntityAvatarController::OnSkeletonComplete->nodeBehaviour is null");
                return;
            }

            skeletonTransform.SetParent(RootTransform, false);

            objTable.Set(SKELETON_GAMEOBJECT_REGISTER_NAME, skeletonGameObject);
            objTable.Set(SKELETON_TRANSFORM_REGISTER_NAME, skeletonTransform);

            objTable.Get<Action<LuaTable>>(SKELETON_LOAD_NAME)?.Invoke(objTable);
        }

        private void CancelLoadSkeleton()
        {
            if (skeletonLoadingID >= 0)
            {
                AssetBridgeData bridgeData = assetBridge.GetBridgeData(skeletonLoadingID);
                objTable.Get<Action<LuaTable, string>>(SKELETON_LOAD_CANCEL_NAME)?.Invoke(objTable, bridgeData.address);

                assetBridge.CancelLoadAsset(skeletonLoadingID);
                skeletonLoadingID = -1;
            }
        }

        public void UnloadSkeleton()
        {
            UnloadAllPart();

            if(skeletonLoadingID>0)
            {
                assetBridge.CancelLoadAsset(skeletonLoadingID);
                skeletonLoadingID = -1;
            }else if(skeletonGameObject!=null)
            {
                objTable.Set<string,GameObject>(SKELETON_GAMEOBJECT_REGISTER_NAME, null);
                objTable.Set<string,Transform>(SKELETON_TRANSFORM_REGISTER_NAME, null);

                GameObject.Destroy(skeletonGameObject);
            }

            objTable.Get<Action<LuaTable>>(SKELETON_UNLOAD_NAME)?.Invoke(objTable);
        }

        public void LoadPart(AvatarPartType partType,string partAddress)
        {
            if(partLoadingDic.TryGetValue(partType,out long loadingID))
            {
                CancelLoadPart(partType, loadingID);
            }

            loadingID = assetBridge.LoadAsset(partAddress, OnPartComplete,partType);
            partLoadingDic.Add(partType, loadingID);
        }

        private void OnPartComplete(string address,UnityObject uObj,SystemObject userData)
        {
            AvatarPartType partType = (AvatarPartType)userData;
            partLoadingDic.Remove(partType);

            AvatarPartData partData = uObj as AvatarPartData;
            if(partData.partType != partType)
            {
                LogUtil.LogError(GetType(), "EntityAvatarController::OnPartAssetComplete->part not same");
                return;
            }
            if(partInstanceDic.TryGetValue(partType,out AvatarPartInstance partInstance))
            {
                AvatarUtil.DisassembleAvatarPart(partInstance);
                partInstanceDic.Remove(partType);
            }

            partInstance = AvatarUtil.AssembleAvatarPart(nodeBehaviour,partData);
            partInstanceDic.Add(partType, partInstance);

            objTable.Get<Action<LuaTable, AvatarPartType>>(PART_LOAD_NAME)?.Invoke(objTable, partType);
        }

        private void CancelLoadPart(AvatarPartType partType,long loadingID)
        {
            AssetBridgeData bridgeData = assetBridge.GetBridgeData(loadingID);
            objTable.Get<Action<LuaTable, string,AvatarPartType>>(PART_LOAD_CANCEL_NAME)?.Invoke(objTable, bridgeData.address,partType);

            assetBridge.CancelLoadAsset(loadingID);

            partLoadingDic.Remove(partType);
        }

        public void UnloadPart(AvatarPartType partType)
        {
            if (partLoadingDic.TryGetValue(partType, out long loadingID))
            {
                assetBridge.CancelLoadAsset(loadingID);
                partLoadingDic.Remove(partType);
            }
            if (partInstanceDic.TryGetValue(partType, out AvatarPartInstance partInstance))
            {
                AvatarUtil.DisassembleAvatarPart(partInstance);
                partInstanceDic.Remove(partType);
            }
        }

        public void UnloadAllPart()
        {
            var keys = partLoadingDic.Keys.ToArray();
            foreach(var key in keys)
            {
                CancelLoadPart(key, partLoadingDic[key]);
            }
            keys = partInstanceDic.Keys.ToArray();
            foreach(var key in keys)
            {
                AvatarUtil.DisassembleAvatarPart(partInstanceDic[key]);
                partInstanceDic.Remove(key);
            }
        }

        protected override void DoDestroy()
        {
        }

        protected override void DoReset()
        {
            throw new NotImplementedException();
        }
    }
}
