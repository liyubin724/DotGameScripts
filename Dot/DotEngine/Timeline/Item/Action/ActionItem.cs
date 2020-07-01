using DotEngine.Timeline.Data;

namespace DotEngine.Timeline.Item
{
    public abstract class ActionItem
    {
        public TimelineContext Context { get; private set; }
        public ActionData Data { get; private set; }
        public float FireTime { get; private set; }

        protected ActionItem()
        {
        }

        public virtual void SetData(TimelineContext context, ActionData actionData, float timeScale)
        {
            Context = context;
            Data = actionData;
            FireTime = actionData.FireTime * timeScale;
        }


        public T GetData<T>() where T:ActionData
        {
            return (T)Data;
        }
    }
}
