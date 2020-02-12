using Dot.Core.TimeLine;

namespace Dot.Core.Entity.TimeLine.Game
{
    [TimeLineMark("Event/Sound", "Special Type", TimeLineExportPlatform.Client)]
    public class PlaySpecialTypeSoundEvent : AEventItem
    {
        public int MusicID { get; set; }

        public override void DoRevert()
        {
           
        }

        public override void Trigger()
        {

        }
    }
}
