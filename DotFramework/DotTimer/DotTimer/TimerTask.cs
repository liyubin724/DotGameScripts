using Dot.Pool;
using System;

namespace Dot.Timer
{
    public class TimerTask : IObjectPoolItem
    {
        private long id = -1;
        public long ID { get => id; }
        
        internal int index = -1;

        internal int intervalInMS = 0;
        private int totalInMS = 0;
        private TimerCallback onStartEvent = null;
        private TimerCallback onIntervalEvent = null;
        private TimerCallback onEndEvent = null;
        private object userData = null;

        internal int remainingInMS = 0;
        private int leftInMS = 0;

        public TimerTask()
        {
        }

        public void SetData(long id, float intervalInSec,
                                                float totalInSec,
                                                TimerCallback startCallback,
                                                TimerCallback intervalCallback,
                                                TimerCallback endCallback,
                                                object callbackData)
        {
            this.id = id;
            intervalInMS = CeilToInt(intervalInSec * 1000);
            if (totalInSec <= 0)
            {
                totalInMS = 0;
            }
            else
            {
                totalInMS = CeilToInt(totalInSec * 1000);
            }
            onStartEvent = startCallback;
            onIntervalEvent = intervalCallback;
            onEndEvent = endCallback;
            userData = callbackData;

            remainingInMS = intervalInMS;
            leftInMS = totalInMS;
        }

        internal void OnReused(float intervalInSec,
                                                float totalInSec,
                                                TimerCallback startCallback,
                                                TimerCallback intervalCallback,
                                                TimerCallback endCallback,
                                                object callbackData)
        {
            intervalInMS = CeilToInt(intervalInSec * 1000);
            if (totalInSec <= 0)
            {
                totalInMS = 0;
            }
            else
            {
                totalInMS = CeilToInt(totalInSec * 1000);
            }
            onStartEvent = startCallback;
            onIntervalEvent = intervalCallback;
            onEndEvent = endCallback;
            userData = callbackData;

            remainingInMS = intervalInMS;
            leftInMS = totalInMS;
        }

        internal void OnTaskStart()
        {
            onStartEvent?.Invoke(userData);
        }

        internal void OnTaskInterval()
        {
            onIntervalEvent?.Invoke(userData);
        }

        internal void OnTaskEnd()
        {
            onEndEvent?.Invoke(userData);
        }

        internal void OnTaskTrigger()
        {
            if (totalInMS > 0)
            {
                leftInMS -= intervalInMS;
            }

            if (onIntervalEvent != null)
            {
                onIntervalEvent(userData);
            }

            if (totalInMS == 0 || leftInMS > 0)
            {
                if (totalInMS == 0 || leftInMS >= intervalInMS)
                {
                    remainingInMS = intervalInMS;
                }
                else
                {
                    remainingInMS = leftInMS;
                }
            }
            else
            {
                if (onEndEvent != null)
                {
                    onEndEvent(userData);
                }
            }
        }

        private int CeilToInt(float f) { return (int)Math.Ceiling(f); }

        public void OnNew()
        {
        }

        public void OnGet()
        {
        }

        public void OnRelease()
        {
            index = -1;
            intervalInMS = 0; ;
            totalInMS = 0;
            remainingInMS = 0;
            leftInMS = 0;
            onStartEvent = null;
            onIntervalEvent = null;
            onEndEvent = null;
            userData = null;
        }

        //----------------------------------------

        internal bool IsValidTask()
        {
            if (intervalInMS <= 0)
            {
                return false;
            }
            if (totalInMS == 0)
            {
                return true;
            }
            else if (totalInMS > 0)
            {
                return leftInMS > 0;
            }
            return false;
        }

        

        internal void OnTrigger()
        {
            if (totalInMS > 0)
            {
                leftInMS -= intervalInMS;
            }

            if (onIntervalEvent != null)
            {
                onIntervalEvent(userData);
            }

            if (totalInMS == 0 || leftInMS > 0)
            {
                if (totalInMS == 0 || leftInMS >= intervalInMS)
                {
                    remainingInMS = intervalInMS;
                }
                else
                {
                    remainingInMS = leftInMS;
                }
            }
            else
            {
                if (onEndEvent != null)
                {
                    onEndEvent(userData);
                }
            }
        }

        internal void OnClear()
        {
            intervalInMS = 0; ;
            totalInMS = 0;
            remainingInMS = 0;
            leftInMS = 0;
            onStartEvent = null;
            onIntervalEvent = null;
            onEndEvent = null;
            userData = null;
        }

        
    }
}
