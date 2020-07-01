using DotEngine.Timeline.Data;
using System.Collections.Generic;

namespace DotEngine.Timeline.Item
{
    public enum GroupState
    {
        None = 0,
        Playing,
        Paused,
        Finished,
    }

    public delegate void OnGroupFinished(Group group);

    public class Group
    {
        public List<Track> tracks = new List<Track>();
        public GroupState State { get; private set; }
        public OnGroupFinished FinishedCallback;

        private float timeLength = 0.0f;
        private TimelineContext context = null;

        public Group(TimelineContext context)
        {
            this.context = context;
        }

        public void SetData(GroupData groupData,float timeScale = 1.0f)
        {
            DoReset();

            timeLength = groupData.TimeLength * timeScale;

            for(int i =0;i<groupData.Tracks.Count;++i)
            {
                Track track = new Track(context);

                track.SetData(groupData.Tracks[i], timeScale);

                tracks.Add(track);
            }
        }

        public bool IsRunning()
        {
            return State == GroupState.Playing || State == GroupState.Paused;
        }

        public void Play()
        {
            if(State!= GroupState.Finished)
            {
                State = GroupState.Playing;
            }
        }

        public void Pause()
        {
            if(State == GroupState.Playing || State == GroupState.None)
            {
                State = GroupState.Paused;
                foreach(var track in tracks)
                {
                    track.DoPause();
                }
            }
        }

        public void Resume()
        {
            if(State == GroupState.Paused)
            {
                State = GroupState.Playing;
                foreach(var track in tracks)
                {
                    track.DoResume();
                }
            }
        }

        public void Stop()
        {
            if (State == GroupState.Playing || State == GroupState.Paused)
            {
                State = GroupState.Finished;
                FinishedCallback?.Invoke(this);

                DoReset();
            }
        }

        public void DoUpdate(float deltaTime)
        {
            if(State != GroupState.Playing)
            {
                return;
            }

            foreach(var track in tracks)
            {
                track.DoUpdate(deltaTime);
            }

            timeLength -= deltaTime;
            if (timeLength <= 0)
            {
                Stop();
            }
        }

        public void DoDestroy()
        {
            DoReset();
            FinishedCallback = null;
            context = null;
        }

        private void DoReset()
        {
            State = GroupState.None;
            foreach(var track in tracks)
            {
                track.DoDestroy();
            }
            tracks.Clear();
            timeLength = 0.0f;
        }
    }
}
