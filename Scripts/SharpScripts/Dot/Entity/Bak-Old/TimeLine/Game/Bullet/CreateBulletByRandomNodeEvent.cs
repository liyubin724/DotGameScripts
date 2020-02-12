using Dot.Core.Entity.Controller;
using Dot.Core.TimeLine;
using Dot.Entity.Node;
using UnityEngine;

namespace Dot.Core.Entity.TimeLine.Game
{
    [TimeLineMark("Event/Bullet", "Random Node", TimeLineExportPlatform.ALL)]
    public class CreateBulletByRandomNodeEvent : AEventItem
    {
        public int BulletConfigID { get; set; }
        public NodeType NodeType { get; set; } = NodeType.None;
        public bool UseEntitySpeed { get; set; } = false;

        public override void DoRevert()
        {
            
        }

        public override void Trigger()
        {
            EntitySkeletonController skeletonController = entity.GetController<EntitySkeletonController>(EntityControllerConst.SKELETON_INDEX);
            if (skeletonController == null) return;

            //BindNodeData[] nodeDatas = skeletonController.GetBindNodes(NodeType);
            //if (nodeDatas != null && nodeDatas.Length > 0)
            //{
            //    Random.InitState((int)Time.realtimeSinceStartup);

            //    BindNodeData nodeData = nodeDatas[Random.Range(0, nodeDatas.Length)];
            //    if (nodeData != null)
            //    {
            //        EntityFactroy.CreateBullet(entity, nodeData, BulletConfigID, UseEntitySpeed);
            //    }
            //}
        }
    }
}
