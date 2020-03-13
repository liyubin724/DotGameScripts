using Dot.Entity.Avatar;
using Dot.Entity.Node;
using Dot.XNodeEx;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Entity.Avatar.Preview
{
    [CreateAssetMenu(fileName = "preview_graph", menuName = "Entity/Avatar/Preview Graph")]
    public class AvatarPreviewGraph : DotNodeGraph
    {
        public List<GameObject> skeletonList = new List<GameObject>();
        public List<AvatarPartData> partList = new List<AvatarPartData>();

        public GameObject[] GetSkeletons()
        {
            return skeletonList.ToArray();
        }

        public AvatarPartData[] GetParts(AvatarPartType partType)
        {
            List<AvatarPartData> parts = new List<AvatarPartData>();
            foreach(var part in partList)
            {
                if(part!=null && part.partType == partType)
                {
                    parts.Add(part);
                }
            }
            return parts.ToArray();
        }

        private GameObject skeletonInstance = null;
        private Dictionary<AvatarPartType, AvatarPartInstance> partInstanceDic = new Dictionary<AvatarPartType, AvatarPartInstance>();

        [HideInInspector]
        [NonSerialized]
        public bool isPreviewing = false;

        internal void CreatePreview(
            GameObject skeletonPrefab,
            AvatarPartData headPart,
            AvatarPartData bodyPart,
            AvatarPartData handPart,
            AvatarPartData feetPart,
            AvatarPartData weaponPart)
        {
            if(isPreviewing)
            {
                DestroyPreview();
            }
            CreateSkeleton(skeletonPrefab);
            AssemblePart(headPart);
            AssemblePart(bodyPart);
            AssemblePart(handPart);
            AssemblePart(feetPart);
            AssemblePart(weaponPart);
            isPreviewing = true;
        }

        internal void DestroyPreview()
        {
            isPreviewing = false;
            partInstanceDic.Clear();
            if(skeletonInstance!=null)
            {
                GameObject.DestroyImmediate(skeletonInstance);
                skeletonInstance = null;
            }
        }

        internal void CreateSkeleton(GameObject skeletonPrefab)
        {
            if (skeletonInstance != null)
            {
                DestroyImmediate(skeletonInstance);
                skeletonInstance = null;
                partInstanceDic.Clear();
            }
            if (skeletonPrefab != null && PrefabUtility.GetPrefabAssetType(skeletonPrefab) == PrefabAssetType.Regular)
            {
                skeletonInstance = GameObject.Instantiate(skeletonPrefab);

                SceneView.lastActiveSceneView.LookAt(skeletonInstance.transform.position);
            }
        }

        internal void AssemblePart(AvatarPartData partData)
        {
            if (partData == null || skeletonInstance == null)
            {
                return;
            }
            if (partInstanceDic.TryGetValue(partData.partType, out AvatarPartInstance partInstance))
            {
                AvatarUtil.DisassembleAvatarPart(partInstance);
                partInstanceDic.Remove(partData.partType);
            }
            NodeBehaviour nodeBehaviour = skeletonInstance.GetComponent<NodeBehaviour>();
            if (nodeBehaviour != null)
            {
                partInstance = AvatarUtil.AssembleAvatarPart(nodeBehaviour, partData);
                partInstanceDic.Add(partData.partType, partInstance);
            }
        }

        internal void DisassemblePart(AvatarPartData partData)
        {
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
    }
}
