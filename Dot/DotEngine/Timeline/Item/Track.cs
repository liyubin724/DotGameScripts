using DotEngine.Timeline.Data;
using System.Collections.Generic;

namespace DotEngine.Timeline.Item
{
    public class Track
    {
        private List<ActionItem> actionItems = new List<ActionItem>();
        private List<DurationActionItem> runningItems = new List<DurationActionItem>();

        private float elapsedTime = 0.0f;
        private TimelineContext context = null;
        private IActionItemFactory itemFactory = null;

        public Track(TimelineContext context)
        {
            this.context = context;
        }

        public void SetData(TrackData trackData, float timeScale = 1.0f)
        {
            itemFactory = context.Get<IActionItemFactory>();

            for (int i = 0; i < trackData.Actions.Count; ++i)
            {
                ActionData actionData = trackData.Actions[i];
                if (actionData.Platform == ActionPlatform.All || actionData.Platform == ActionPlatform.Client)
                {
                    ActionItem actionItem = itemFactory.RetainItem(actionData);
                    if (actionItem == null)
                    {
                        continue;
                    }
                    actionItem.SetData(context, actionData, timeScale);

                    actionItems.Add(actionItem);
                }
            }
        }

        public void DoUpdate(float deltaTime)
        {
            if (actionItems.Count == 0 && runningItems.Count == 0)
            {
                return;
            }

            elapsedTime += deltaTime;

            while (actionItems.Count > 0)
            {
                ActionItem actionItem = actionItems[0];
                if (actionItem.FireTime <= elapsedTime)
                {
                    if (actionItem is DurationActionItem durationActionItem)
                    {
                        durationActionItem.DoEnter();
                        runningItems.Add(durationActionItem);
                    } else if (actionItem is EventActionItem eventActionItem)
                    {
                        eventActionItem.Trigger();
                        itemFactory.ReleaseItem(eventActionItem);
                    }
                    actionItems.RemoveAt(0);
                } else
                {
                    break;
                }
            }

            if (runningItems.Count > 0)
            {
                for (int i = 0; i < runningItems.Count;)
                {
                    DurationActionItem durationActionItem = runningItems[i];
                    durationActionItem.DoUpdate(deltaTime);

                    if (durationActionItem.EndTime <= elapsedTime)
                    {
                        durationActionItem.DoExit();

                        runningItems.RemoveAt(i);
                        itemFactory.ReleaseItem(durationActionItem);
                    }
                    else
                    {
                        ++i;
                    }
                }
            }
        }

        public void DoDestroy()
        {
            for(int i = runningItems.Count-1;i>=0;--i)
            {
                var item = runningItems[0];
                item.DoExit();

                itemFactory.ReleaseItem(item);
            }
            runningItems.Clear();

            for(int i = actionItems.Count-1;i>=0;--i)
            {
                var item = actionItems[i];

                itemFactory.ReleaseItem(item);
            }
            actionItems.Clear();

            elapsedTime = 0.0f;
            itemFactory = null;
            context = null;
        }

        public void DoPause()
        {
            foreach (var item in runningItems)
            {
                item.DoPause();
            }
        }

        public void DoResume()
        {
            foreach(var item in runningItems)
            {
                item.DoResume();
            }
        }

    }
}
