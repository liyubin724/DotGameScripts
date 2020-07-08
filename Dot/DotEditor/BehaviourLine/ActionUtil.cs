using DotEngine.BehaviourLine.Action;
using System;
using System.Reflection;

namespace DotEditor.BehaviourLine
{
    public static class ActionUtil
    {
        public static ActionData Copy(this ActionData fromAction)
        {
            if (fromAction == null)
            {
                return null;
            }

            Type actionType = fromAction.GetType();
            ActionData data = (ActionData)Activator.CreateInstance(actionType);

            FieldInfo[] fields = actionType.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var field in fields)
            {
                object value = field.GetValue(fromAction);
                field.SetValue(data, value);
            }

            return data;
        }
    }
}
