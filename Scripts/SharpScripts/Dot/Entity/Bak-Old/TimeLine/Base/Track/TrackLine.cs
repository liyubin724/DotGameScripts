﻿using Dot.Core.Entity;
using System.Collections.Generic;

namespace Dot.Core.TimeLine
{
    public class TrackLine : AEntityEnv
    {
        public string Name { get; set; } = "Track";
        public List<AItem> items = new List<AItem>();

        private List<AItem> waitingItems = new List<AItem>();
        private List<AItem> runningItems = new List<AItem>();
        private float elapsedTime = 0f;

        public TrackGroup Group { get; set; }

        public override void Initialize(EntityContext contexts, EntityObject entity)
        {
            base.Initialize(contexts, entity);
            items.ForEach((item) =>
            {
                item.Initialize(contexts, entity);
            });
        }

        public void DoUpdate(float deltaTime)
        {
            if(elapsedTime == 0f && waitingItems.Count ==0 && items.Count>0)
            {
                waitingItems.AddRange(items);
            }

            float previousTime = elapsedTime;
            elapsedTime += deltaTime;

            if (runningItems.Count == 0 && waitingItems.Count == 0)
            {
                return;
            }
            while(waitingItems.Count>0)
            {
                AItem item = waitingItems[0];
                if(item.FireTime>elapsedTime)
                {
                    break;
                }
                if(item.FireTime>=previousTime && item.FireTime<elapsedTime)
                {
                    runningItems.Add(item);
                    waitingItems.RemoveAt(0);
                }
            }
            
            for (int i=runningItems.Count-1;i>=0;--i)
            {
                AItem item = runningItems[i];
                if (item is AEventItem eventItem)
                {
                    if (previousTime <= eventItem.FireTime && elapsedTime > eventItem.FireTime)
                    {
                        eventItem.Trigger();
                        if (eventItem.CanRevert)
                        {
                            Group.AddRevertItem(eventItem);
                        }
                    }
                    runningItems.RemoveAt(i);
                }else if(item is AActionItem actionItem)
                {
                    if (previousTime <= actionItem.FireTime && elapsedTime > actionItem.FireTime)
                    {
                        actionItem.Enter();
                    }
                    else if (previousTime <= actionItem.EndTime && elapsedTime > actionItem.EndTime)
                    {
                        actionItem.Exit();
                        runningItems.RemoveAt(i);
                    }
                    else if (previousTime >= actionItem.FireTime && elapsedTime <= actionItem.EndTime)
                    {
                        actionItem.DoUpdate(deltaTime);
                    }else
                    {
                        runningItems.RemoveAt(i);
                    }
                }
            }
        }
        
        public void Stop()
        {
            runningItems.ForEach((item) =>
            {
                if (item is AActionItem actionItem)
                {
                    actionItem.Stop();
                }
            });
        }

        public void Pause()
        {
            runningItems.ForEach((item) =>
            {
                if (item is AActionItem actionItem)
                {
                    actionItem.Pause();
                }
            });
        }

        public void Resume()
        {
            runningItems.ForEach((item) =>
            {
                if (item is AActionItem actionItem)
                {
                    actionItem.Pause();
                }
            });
        }

        public override void DoReset()
        {
            runningItems.ForEach((item) =>
            {
                item.DoReset();
            });
            base.DoReset();
            runningItems.Clear();
            waitingItems.Clear();
            elapsedTime = 0f;
        }
    }
}
