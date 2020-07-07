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

        public TracklineDrawer(TimelineDrawer drawer)
        {
            ParentDrawer = drawer;
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


        }

        public void OnDrawProperty(Rect rect)
        {

        }
    }
}
