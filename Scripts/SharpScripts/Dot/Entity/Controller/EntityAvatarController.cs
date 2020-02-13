using Dot.Asset;
using Dot.Entity.Avatar;
using Dot.Entity.Node;
using Dot.Log;
using System.Collections.Generic;
using UnityEngine;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace Dot.Entity.Controller
{
    public class EntityAvatarController : EntityControllerBase
    {
        private static readonly string SKELETON_LOAD_COMPLETE_NAME = "OnSkeletonComplete";
        private static readonly string PART_LOAD_COMPLETE_NAME = "OnPartComplete";

        private Transform rootTransform = null;
        private Transform RootTransform
        {
            get
            {
                if(rootTransform==null)
                {
                    EntityVirtualViewController vvc = Entity.GetController<EntityVirtualViewController>(EntityControllerType.VirtualView);
                    rootTransform = vvc.RootTransfrom;
                }
                return rootTransform;
            }
        }

        private AssetBridge assetBridge = null;
        
        private NodeBehaviour nodeBehaviour = null;
        private long skeletonLoadingID = -1;

        private Dictionary<AvatarPartType, AvatarPartInstance> partInstanceDic = new Dictionary<AvatarPartType, AvatarPartInstance>();
        private Dictionary<AvatarPartType, long> partLoadingDic = new Dictionary<AvatarPartType, long>();

        protected override void DoInit()
        {
            assetBridge = new AssetBridge();
        }

        protected override void DoReset()
        {
            UnloadAllPart();
            UnloadSkeleton();
            assetBridge.Dispose();
            assetBridge = null;
        }

        public NodeData GetBindNodeData(string nodeName)
        {
            if(nodeBehaviour == null)
            {
                LogUtil.LogError(LOGGER_NAME, "EntityAvatarController::GetBindNode->nodeBehaviour is not been loaded");
                return null;
            }

            return nodeBehaviour.GetNode(NodeType.BindNode, nodeName);
        }

        private void BindTransfrom(string nodeName,Transform transform,
            Vector3 positionOffset,Vector3 rotationOffset)
        {
            NodeData bindNodeData = GetBindNodeData(nodeName);
            if(bindNodeData!=null)
            {
                transform.SetParent(bindNodeData.transform, false);
                transform.localPosition = positionOffset;
                transform.localRotation = Quaternion.Euler(rotationOffset);
            }
        }

        public void LoadSkeleton(string skeletonAddress)
        {
            if(skeletonLoadingID>=0)
            {
                assetBridge.CancelLoadAsset(skeletonLoadingID);
                skeletonLoadingID = -1;
            }
            skeletonLoadingID = assetBridge.InstanceAsset(skeletonAddress, OnSkeletonAssetComplete);
        }

        private void OnSkeletonAssetComplete(string address,UnityObject uObj,SystemObject userData)
        {
            if(nodeBehaviour!=null)
            {
                UnloadAllPart();
                UnloadSkeleton();
            }

            GameObject gObj = uObj as GameObject;
            nodeBehaviour = gObj.GetComponent<NodeBehaviour>();

        }

        private void OnPartAssetComplete(string address,UnityObject uObj,SystemObject userData)
        {

        }

        public void UnloadSkeleton()
        {
            if(nodeBehaviour!=null)
            {
                UnloadAllPart();


            }
        }

        public void UnloadAllPart()
        {
            foreach(var kvp in partLoadingDic)
            {
                assetBridge.CancelLoadAsset(kvp.Value);
            }
            partLoadingDic.Clear();

            foreach(var kvp in partInstanceDic)
            {
                UnloadPart(kvp.Value);
            }
            partInstanceDic.Clear();
        }

        public void UnloadPart(AvatarPartType partType)
        {
            if(partLoadingDic.TryGetValue(partType,out long loadingID))
            {
                assetBridge.CancelLoadAsset(loadingID);
                partLoadingDic.Remove(partType);
            }
            if(partInstanceDic.TryGetValue(partType,out AvatarPartInstance partInstance))
            {
                UnloadPart(partInstance);
                partInstanceDic.Remove(partType);
            }
        }

        public void UnloadPart(AvatarPartInstance partInstance)
        {

        }

        public void LoadPart(string partAssetPath)
        {

        }
    }
}
