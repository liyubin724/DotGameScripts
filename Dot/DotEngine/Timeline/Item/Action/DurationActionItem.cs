using DotEngine.Timeline.Data;

namespace DotEngine.Timeline.Item
{
    public abstract class DurationActionItem : ActionItem
    {
        public float DurationTime { get; private set; }

        public float EndTime { get => FireTime + DurationTime; }

        public override void SetData(TimelineContext context, ActionData actionData, float timeScale)
        {
            base.SetData(context, actionData, timeScale);

            DurationActionData durationActionData = (DurationActionData)actionData;
            DurationTime = durationActionData.DurationTime * timeScale;
        }

        public abstract void DoEnter();
        public abstract void DoExit();
        public abstract void DoUpdate(float deltaTime);
        public virtual void DoPause() { }
        public virtual void DoResume() { }

        public override void DoReset()
        {
            DurationTime = 0.0f;

            base.DoReset();
        }
    }
}
