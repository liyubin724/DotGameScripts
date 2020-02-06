using Dot.Core.Entity.Controller;
using Dot.Core.TimeLine;
using Dot.Entity.Node;

namespace Dot.Core.Entity.TimeLine.Game
{
    [TimeLineMark("Event/Sound", "Bind Node", TimeLineExportPlatform.Client)]
    public class PlayBindNodeSoundEvent : AEventItem
    {
        public int MusicID { get; set; }
        public NodeType NodeType { get; set; } = NodeType.None;
        public bool IsAllNode { get; set; } = false;
        public int NodeIndex { get; set; }

        public override void DoRevert()
        {
        }

        public override void Trigger()
        {
            EntitySkeletonController skeletonController = entity.GetController<EntitySkeletonController>(EntityControllerConst.SKELETON_INDEX);
            if (skeletonController == null) return;

            //if(IsAllNode)
            //{
            //    BindNodeData[] nodeDatas = skeletonController.GetBindNodes(NodeType);
            //    if(nodeDatas!=null && nodeDatas.Length>0)
            //    {
            //        foreach(var nodeData in nodeDatas)
            //        {
            //        }
            //    }
            //}else
            //{
            //    BindNodeData nodeData = skeletonController.GetBindNodeData(NodeType, NodeIndex);
            //    if(nodeData!=null)
            //    {
            //    }
            //}
        }
    }
}
