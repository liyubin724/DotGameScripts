using System.Collections.Generic;

namespace DotEngine.Timeline
{
    public class TrackGroup
    {
        public float Length { get; set; }
        public List<ActionTrack> Tracks = new List<ActionTrack>();
    }
}
