namespace DotEngine.Timeline.Data
{
    public enum ActionPlatform
    {
        All = 0,
        Client,
        Server,
    }

    public class ActionData
    {
        public int Index = 0;
        public int Id = 0;
        public ActionPlatform Platform = ActionPlatform.All;
        public float FireTime = 0.0f;
    }
}
