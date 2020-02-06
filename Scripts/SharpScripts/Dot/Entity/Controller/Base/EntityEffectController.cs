using Dot.Core.Effect;
using Dot.Entity.Node;
using System.Collections.Generic;

namespace Dot.Core.Entity.Controller
{
    public class EntityEffectController : AEntityController
    {
        private EntitySkeletonController skeletonController = null;

        private List<EffectController> effectList = new List<EffectController>();

        protected override void DoInit()
        {
            base.DoInit();

            skeletonController = entity.GetController<EntitySkeletonController>(EntityControllerConst.SKELETON_INDEX);
        }

        public EffectController BindEffect(NodeType nodeType, int nodeIndex, int effectConfigID, EffectScenarioType scenarioType, bool isAutoRelease)
        {
            //if (skeletonController == null) return null;

            //BindNodeData nodeData = skeletonController.GetBindNodeData(nodeType, nodeIndex);
            //if (nodeData == null) return null;

            //EffectController effect = CreateEffectController(nodeData, effectConfigID, scenarioType, isAutoRelease);
            //effectList.Add(effect);

            //return effect;
            return null;
        }

        public EffectController[] BindEffect(NodeType nodeType, int effectConfigID, EffectScenarioType scenarioType,bool isAutoRelease)
        {
            //if (skeletonController == null) return null;
            //BindNodeData[] nodeDatas = skeletonController.GetBindNodes(nodeType);
            //if(nodeDatas!=null && nodeDatas.Length>0)
            //{
            //    EffectController[] effects = new EffectController[nodeDatas.Length];
            //    for(int i =0;i<nodeDatas.Length;i++)
            //    {
            //        EffectController effect = CreateEffectController(nodeDatas[i], effectConfigID, scenarioType, isAutoRelease);
            //        effectList.Add(effect);
            //        effects[i] = effect;
            //    }
            //    return effects;
            //}
            
            return null;
        }

        private EffectController CreateEffectController(NodeData nodeData,int effectConfigID, EffectScenarioType scenarioType, bool isAutoRelease)
        {
            //EffectConfigData data = ConfigManager.GetInstance().GetEffectConfig(effectConfigID);

            //EffectController effect = EffectManager.GetInstance().GetEffect(data.address, scenarioType, isAutoRelease);
            //effect.isAutoPlayWhenEnable = data.isAutoPlay;
            //effect.lifeTime = data.lifeTime;
            //effect.stopDelayTime = data.stopDelayTime;

            //if (isAutoRelease)
            //{
            //    effect.effectFinished += OnAutoReleaseEffectComplete;
            //}else
            //{
            //    effect.effectFinished += OnEffectComplete;
            //}
            //return effect;
            return null;
        }

        private void OnEffectComplete(EffectController effect)
        {
            effect.effectFinished -= OnEffectComplete;
            effectList.Remove(effect);
            EffectManager.GetInstance().ReleaseEffect(effect);
        }

        private void OnAutoReleaseEffectComplete(EffectController effect)
        {
            effect.effectFinished -= OnAutoReleaseEffectComplete;
            effectList.Remove(effect);
        }

    }
}
