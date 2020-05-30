using Dot.Asset;
using Dot.Entity.Avatar;
using Dot.Entity.Node;
using Dot.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using UnityEngine;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace Dot.Entity.Controller
{
    class EntityAvatarSkeletonData
    {
        private string address = null;
        public string Address { get => address; }

        private int loaderID = -1;
        public int LoaderID { get => loaderID; }

        private GameObject gObject = null;
        public GameObject GObject { get => gObject; }

        private NodeBehaviour nodeBehaviour = null;
        public NodeBehaviour NodeBeh { get => nodeBehaviour; }

        public bool IsLoading()
        {
            return loaderID > 0;
        }

        public bool IsLoaded()
        {
            return gObject != null && loaderID < 0;
        }

        public void SetLoader(string address, int loaderID)
        {
            this.address = address;
            this.loaderID = loaderID;
        }

        public void SetSkeleton(GameObject gObj)
        {
            loaderID = -1;
            gObject = gObj;
            nodeBehaviour = gObject.GetComponent<NodeBehaviour>();
        }

        public void ResetSkeleton()
        {
            address = null;
            loaderID = -1;
            gObject = null;
            nodeBehaviour = null;
        }
    }

    class EntityAvatarPartData
    {
        private string address;
        public string Address { get => address; }

        private int loaderID = -1;
        public int LoaderID { get => loaderID; }

        private AvatarPartData partData;
        public AvatarPartData PartData { get => partData; }

        private AvatarPartInstance partInstance;
        public AvatarPartInstance PartInstance { get => partInstance; }

        public bool IsLoading()
        {
            return loaderID > 0;
        }

        public bool IsLoaded()
        {
            return partData != null && partInstance == null;
        }

        public bool IsAssembled()
        {
            return partData != null && partInstance != null;
        }

        public void SetLoader(string address, int loaderID)
        {
            this.address = address;
            this.loaderID = loaderID;
        }

        public void SetPart(AvatarPartData partData)
        {
            loaderID = -1;
            this.partData = partData;
        }

        public void SetPartInstance(AvatarPartInstance instance)
        {
            partInstance = instance;
        }

    }

    public class EntityAvatarController : EntityController
    {
        private AssetBridge assetBridge = null;

        private EntityAvatarSkeletonData skeletonData = new EntityAvatarSkeletonData();

        private List<EntityAvatarPartData> partDatas = new List<EntityAvatarPartData>();

        protected Transform RootTransform
        {
            get
            {
                return entity.GetController<EntityGameObjectController>().RootTransform;
            }
        }

        public NodeBehaviour GetNodeBehaviour()
        {
            return skeletonData.NodeBeh;
        }

        protected override void OnInitialized()
        {
            assetBridge = new AssetBridge();
        }

        public void LoadSkeleton(string skeletonAddress)
        {
            if(skeletonData.Address == skeletonAddress)
            {
                if(skeletonData.IsLoaded())
                {
                    SendEvent(EntityEventConst.ENTITY_SKELETON_LOAED);
                }
                return;
            }

            if(skeletonData.IsLoading())
            {
                assetBridge.CancelLoad(skeletonData.LoaderID);
            }else if(skeletonData.IsLoaded())
            {
                UnloadSkeleton();
            }

            skeletonData.ResetSkeleton();

            int uniqueID = assetBridge.InstanceAsset(skeletonAddress, OnLoadSkeletonCompleted);
            skeletonData.SetLoader(skeletonAddress, uniqueID);
        }

        public void UnloadSkeleton()
        {

        }

        public void LoadPart(string partAddress)
        {
            //if(partDatas.IndexOf(partAddress)<0)
            //{
            //    assetBridge.LoadAsset(partAddress, OnLoadPartCompleted);
            //}
        }

        public void LoadParts(string[] partAddresses)
        {
            foreach(var address in partAddresses)
            {
                LoadPart(address);
            }
        }

        public void UnloadPart(string partAddress)
        {

        }

        public void UnloadAllParts()
        {

        }

        public void LoadSkeletonAndParts(string skeletonAddress, string[] partAddresses)
        {
            LoadSkeleton(skeletonAddress);
            LoadParts(partAddresses);
        }

        private void OnLoadSkeletonCompleted(AssetHandler assetHandler)
        {
            skeletonData.SetSkeleton(assetHandler.UObject as GameObject);

            Transform transform = skeletonData.GObject.transform;
            transform.SetParent(RootTransform);
            transform.localPosition = Vector3.zero;

            SendEvent(EntityEventConst.ENTITY_SKELETON_LOAED);
        }

        private void OnLoadPartCompleted(AssetHandler assetHandler)
        {

        }

    }
}
