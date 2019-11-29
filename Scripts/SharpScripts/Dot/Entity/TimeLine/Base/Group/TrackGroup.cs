﻿using Dot.Core.Entity;
using System.Collections.Generic;

namespace Dot.Core.TimeLine
{
    public class TrackGroup : AEntityEnv
    {
        public string Name { get; set; } = "Group";
        public float TotalTime { get; set; }
        public bool CanRevert { get; set; } = true;
        public readonly List<TrackLine> tracks = new List<TrackLine>();

        public TrackController Data { get; set; }
        private List<AEventItem> revertEventItems = new List<AEventItem>();
        public override void Initialize(EntityContext contexts, EntityObject entity)
        {
            base.Initialize(contexts, entity);
            tracks?.ForEach((track) =>
            {
                track.Initialize(contexts, entity);
                track.Group = this;
            });
        }

        private float elapsedTime = 0.0f;
        public void DoUpdate(float deltaTime)
        {
            if(isInit)
            {
                if(elapsedTime == 0)
                {
                    Data?.OnGroupStart(this);
                }

                tracks?.ForEach((track) =>
                {
                    track?.DoUpdate(deltaTime);
                });

                elapsedTime += deltaTime;
                if(elapsedTime >= TotalTime)
                {
                    Stop(false);
                }
            }
        }

        public override void DoReset()
        {
            elapsedTime = 0.0f;
            revertEventItems.Clear();
            base.DoReset();
        }

        public void Stop(bool isForce)
        {
            tracks?.ForEach((track) =>
            {
                track?.Stop();
            });

            if(CanRevert)
            {
                for(int i = revertEventItems.Count-1;i>=0;--i)
                {
                    revertEventItems[i].DoRevert();
                }
            }
            if(!isForce)
                Data?.OnGroupFinish(this);
        }

        internal void AddRevertItem(AEventItem item)
        {
            revertEventItems.Add(item);
        }
    }
}
