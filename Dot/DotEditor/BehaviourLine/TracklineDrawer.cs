using DotEngine.BehaviourLine.Track;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DotEditor.BehaviourLine
{
    public class TracklineDrawer
    {
        public TimelineDrawer ParentDrawer { get; private set; }
        public TracklineData Data { get; private set; }

        private int index = 0;
        private List<ActionDrawer> actionDrawers = new List<ActionDrawer>();
        private ActionMenu actionMenu = null;

        private bool isSelected = false;
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    if(isSelected)
                    {

                    }else
                    {

                    }
                }
            }
        }

        public TracklineDrawer(TimelineDrawer drawer)
        {
            ParentDrawer = drawer;
            actionMenu = new ActionMenu();
        }

        public void SetData(int index,TracklineData data)
        {
            this.index = index;
            Data = data;

            actionDrawers.Clear();

            foreach(var d in data.Actions)
            {
                ActionDrawer drawer = new ActionDrawer(this);
                drawer.SetData(d);

                actionDrawers.Add(drawer);
            }
        }

        public void OnDrawGUI(Rect rect)
        {
            foreach(var drawer in actionDrawers)
            {
                drawer.OnDrawGUI(rect);
            }

            int eventBtn = Event.current.button;
            EventType eventType = Event.current.type;
            Vector2 mPos = Event.current.mousePosition;
            bool isContains = rect.Contains(Event.current.mousePosition);
            if(isContains && eventType == EventType.MouseUp)
            {
                if(eventBtn == 0)
                {
                    IsSelected = true;
                    Event.current.Use();
                }else if (eventBtn == 1)
                {
                    LineSetting setting = LineSetting.Setting;
                    actionMenu.ShowMenu((actionData) =>
                    {
                        float fireTime = (mPos.x + setting.ScrollPosX) / setting.WidthForSecond;
                        actionData.Index = setting.GetActionIndex();
                        actionData.FireTime = fireTime;

                        Data.Actions.Add(actionData);
                        ActionDrawer itemDrawer = new ActionDrawer(this);
                        itemDrawer.SetData(actionData);

                        actionDrawers.Add(itemDrawer);
                    });
                    Event.current.Use();
                }
            }
        }

        public void OnDrawProperty(Rect rect)
        {

        }
    }
}
