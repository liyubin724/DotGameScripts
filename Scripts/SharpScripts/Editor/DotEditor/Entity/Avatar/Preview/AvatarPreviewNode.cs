using Dot.Entity.Avatar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using UnityEditor;
using Dot.Entity.Node;

namespace DotEditor.Entity.Avatar.Preview
{
    public class AvatarPreviewNode : XNode.Node {
        public GameObject cameraPrefab = null;

        [Input]
        public GameObject skeletonPrefab = null;
        [Input]
        public AvatarPartData headPart = null;
        [Input]
        public AvatarPartData bodyPart = null;
        [Input]
        public AvatarPartData handPart = null;
        [Input]
        public AvatarPartData feetPart = null;
        [Input]
        public AvatarPartData weaponPart = null;

        [Output]
        public RenderTexture previewTexture = null;

        private GameObject skeletonInstance = null;
        private GameObject cameraInstance = null;
        private Camera camera = null;

        private Dictionary<AvatarPartType, AvatarPartInstance> partInstanceDic = new Dictionary<AvatarPartType, AvatarPartInstance>();

        protected override void Init()
        {
            base.Init();
        }

        private void CreateCameraInstance()
        {
            if(cameraInstance!=null)
            {
                if(skeletonInstance!=null)
                {
                    skeletonInstance.transform.SetParent(null, false);
                }
                GameObject.DestroyImmediate(camera);
                cameraInstance = null;
                camera = null;
            }
            if(cameraPrefab!=null)
            {
                cameraInstance = GameObject.Instantiate(cameraPrefab);
            }else
            {
                cameraInstance = new GameObject();
                cameraInstance.AddComponent<Camera>();
            }
            cameraInstance.name = "Avatar Preview Camera";
            Camera[] cameras = cameraInstance.GetComponentsInChildren<Camera>();
            if(cameras!=null && cameras.Length>0)
            {
                camera = cameras[0];
            }
        }

        private void CreateSkeletonInstance()
        {
            if(skeletonInstance!=null)
            {
                DestroyImmediate(skeletonInstance);
                skeletonInstance = null;
            }

            GameObject sPrefab = GetInputValue<GameObject>("skeletonPrefab");
            if(sPrefab!=null && PrefabUtility.GetPrefabAssetType(sPrefab) == PrefabAssetType.Regular)
            {
                skeletonInstance = GameObject.Instantiate(skeletonPrefab);
            }
            if(skeletonInstance!=null)
            {
                Transform sParent = cameraInstance.transform.Find("Parent");
                skeletonInstance.transform.SetParent(sParent, false);

                AssembeAllPart();
            }
        }

        private void AssembeAllPart()
        {
            if(skeletonInstance!=null)
            {
                AssemblePart("headPart");
                AssemblePart("bodyPart");
                AssemblePart("handPart");
                AssemblePart("feetPart");
                AssemblePart("weaponPart");
            }
        }

        private void AssemblePart(string partName)
        {
            AvatarPartData partData = GetInputValue<AvatarPartData>(partName);
            if(partData == null || skeletonInstance == null)
            {
                return;
            }
            if(partInstanceDic.TryGetValue(partData.partType,out AvatarPartInstance partInstance))
            {
                AvatarUtil.DisassembleAvatarPart(partInstance);
                partInstanceDic.Remove(partData.partType);
                partInstance = null;
            }
            NodeBehaviour nodeBehaviour = skeletonInstance.GetComponent<NodeBehaviour>();
            if(nodeBehaviour!=null)
            {
                partInstance = AvatarUtil.AssembleAvatarPart(nodeBehaviour, partData);
                partInstanceDic.Add(partData.partType, partInstance);
            }
        }

        private void DisassemblePart(string partName)
        {
            AvatarPartData partData = GetInputValue<AvatarPartData>(partName);
            if (partData == null || skeletonInstance == null)
            {
                return;
            }
            if (partInstanceDic.TryGetValue(partData.partType, out AvatarPartInstance partInstance))
            {
                AvatarUtil.DisassembleAvatarPart(partInstance);
                partInstanceDic.Remove(partData.partType);
            }
        }

        public override void OnCreateConnection(NodePort from, NodePort to)
        {
            Debug.Log($"OnCreateConnection->From={from.fieldName},to={to.fieldName}");
            base.OnCreateConnection(from, to);
        }

        public override void OnRemoveConnection(NodePort port)
        {
            Debug.Log($"OnRemoveConnection->port={port.fieldName}");
            base.OnRemoveConnection(port);
        }

        // Return the correct value of an output port when requested
        public override object GetValue(NodePort port) {
            if(port.fieldName == "tf")
            {
                return 1111.00f;
            }
		    return null; // Replace this
	    }
    }

}