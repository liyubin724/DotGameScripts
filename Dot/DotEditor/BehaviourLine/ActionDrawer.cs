using DotEngine.BehaviourLine.Action;
using System.Reflection;
using UnityEngine;

namespace DotEditor.BehaviourLine
{
    public class ActionDrawer
    {
        public ActionData Data { get; private set; }
        public TracklineDrawer ParentDrawer { get; private set; }
        public string BriefName
        {
            get
            {
                ActionNameAttribute attr = Data.GetType().GetCustomAttribute<ActionNameAttribute>();
                if(attr == null)
                {
                    return string.Empty;
                }
                return attr.BriefName;
            }
        }

        public string DetailName
        {
            get
            {
                ActionNameAttribute attr = Data.GetType().GetCustomAttribute<ActionNameAttribute>();
                if(attr == null)
                {
                    return string.Empty;
                }
                return attr.DetailName;
            }
        }

        private bool isSelected = false;
        public bool IsSelected
        {
            get;set;
        }

        public ActionDrawer(TracklineDrawer drawer)
        {
            ParentDrawer = drawer;
        }

        public void SetData(ActionData data)
        {
            Data = data;

        }

        public void OnDrawGUI(Rect rect)
        {
            LineSetting config = LineSetting.Setting;

            Rect itemRect = Rect.zero;
            itemRect.x = Data.FireTime * config.WidthForSecond - config.ScrollPosX;
            itemRect.y = rect.y;
            itemRect.height = config.TracklineHeight;
            itemRect.width = config.TimeStepWidth;

            if(Data is DurationActionData durationActionData)
            {
                itemRect.width = Mathf.Max(config.TimeStepWidth, durationActionData.DurationTime * config.WidthForSecond);
            }
            GUI.Label(itemRect, BriefName, IsSelected ? "flow node 6" : "flow node 5");

            int eventBtn = Event.current.button;
            EventType eventType = Event.current.type;
            bool isContains = itemRect.Contains(Event.current.mousePosition);

        }

        public void OnDrawProperty(Rect rect)
        {

        }

    }
}
