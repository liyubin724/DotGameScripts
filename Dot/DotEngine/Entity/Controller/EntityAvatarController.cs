using Dot.Asset;
using Dot.Entity.Avatar;
using Dot.Entity.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace Dot.Entity.Controller
{
    public class EntityAvatarController : EntityController
    {
        private GameObject skeletonGameObject = null;
        private NodeBehaviour nodeBehaviour = null;
        private AssetBridge assetBridge = null;

        private int skeletonLoaderFlag = -1;
        private List<AvatarPartData> waitingAssemblyPartDatas = new List<AvatarPartData>();
        private Dictionary<string, int> partLoaderFlagDic = new Dictionary<string, int>();

        private Dictionary<AvatarPartType, AvatarPartInstance> assembliedPartInstanceDic = new Dictionary<AvatarPartType, AvatarPartInstance>();

        public GameObject SkeletonGameObject
        {
            get
            {
                return skeletonGameObject;
            }
        }

        public NodeBehaviour Node
        {
            get
            {
                return nodeBehaviour;
            }
        }

        protected Transform RootTransform
        {
            get
            {
                return entity.GetController<EntityGameObjectController>().RootTransform;
            }
        }

        protected override void OnInitialized()
        {
            assetBridge = new AssetBridge();
        }

        public void LoadSkeleton(string skeletonAddress)
        {
            if(skeletonLoaderFlag>0)
            {
                assetBridge.CancelLoad(skeletonLoaderFlag);
            }
            skeletonLoaderFlag = assetBridge.InstanceAsset(skeletonAddress, OnLoadSkeletonCompleted);
        }

        public void LoadSkeletonAndParts(string skeletonAddress,string[] partAddresses)
        {
            LoadSkeleton(skeletonAddress);
            LoadParts(partAddresses);
        }

        public void LoadPart(string partAddress)
        {

        }

        public void LoadParts(string[] partAddresses)
        {

        }

        private void OnLoadSkeletonCompleted(string address, UnityObject uObj, SystemObject userData)
        {

        }

    }
}
