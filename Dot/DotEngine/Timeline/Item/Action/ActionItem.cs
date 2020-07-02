using DotEngine.Timeline.Data;

namespace DotEngine.Timeline.Item
{
    public abstract class ActionItem
    {
        public ActionData Data { get; private set; }
        public float FireTime { get; private set; }

        protected TimelineContext Context { get; private set; }

        protected ActionItem()
        {
        }

        public virtual void SetData(TimelineContext context, ActionData actionData, float timeScale)
        {
            Context = context;
            Data = actionData;
            FireTime = actionData.FireTime * timeScale;
        }

        public virtual void DoReset()
        {
            Data = null;
            FireTime = 0.0f;
            Context = null;
        }

        public T GetData<T>() where T:ActionData
        {
            return (T)Data;
        }
    }
}
