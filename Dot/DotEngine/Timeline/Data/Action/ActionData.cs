using System;
using System.Reflection;

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

        public ActionData Copy()
        {
            Type actionType = GetType();
            ActionData data = (ActionData)Activator.CreateInstance(actionType);

            FieldInfo[] fields = actionType.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var field in fields)
            {
                object value = field.GetValue(this);
                field.SetValue(data, value);
            }

            return data;
        }
    }
}
