using Dot.Asset;
using Dot.Entity.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Dot.Entity.Controller
{
    public class EntityAvatarController : EntityController
    {
        private AssetBridge assetBridge = null;
        private NodeBehaviour nodeBehaviour = null;

        protected override void OnInitialized()
        {
            assetBridge = new AssetBridge();
        }

        protected Transform RootTransform
        {
            get
            {
                return entity.GetController<EntityGameObjectController>().RootTransform;
            }
        }


        public void LoadSkeleton(string skeletonAddress)
        {

        }

        private void OnLoadSkeletonComplete()
        {

        }

        public void LoadPart(string partAddress)
        {

        }

        private void OnLoadPartComplete()
        {

        }

    }
}
