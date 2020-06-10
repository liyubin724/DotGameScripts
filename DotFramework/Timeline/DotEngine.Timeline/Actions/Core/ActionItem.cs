namespace DotEngine.Timeline
{
    public abstract class ActionItem
    {
        public int Index = 0;
        public int ActionID = -1;
        public ActionPlatform Platform = ActionPlatform.All;
        public float FireTime = 0.0f;
    }
}
