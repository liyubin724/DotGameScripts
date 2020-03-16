using System.Collections.Generic;

namespace Dot.Timer
{
    internal delegate void OnTimerWheelTrigger(int index, List<TimerTask> taskList);
    internal delegate void OnTimerWheelOut(int index);

    /// <summary>
    /// 时间轮定时器
    /// </summary>
    internal sealed class TimerWheel
    {
        //用于多层时间轮层次索引
        private int index = 0;
        private int tickInMS = 0;
        private int slotSize = 0;

        internal int TickInMS { get => tickInMS; }
        internal int TotalTickInMS{ get => tickInMS * slotSize; }

        private int currentSlotIndex = 0;
        private List<TimerTask>[] slotArr = null;
        private List<TimerTask> willTriggerTaskList = new List<TimerTask>();

        internal OnTimerWheelTrigger wheelTriggerEvent = null;
        internal OnTimerWheelOut wheelOutEvent = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="index">时间轮序号</param>
        /// <param name="tickInMS">一刻度的时长，以毫秒计</param>
        /// <param name="slotSize">总的刻度数</param>
        internal TimerWheel(int index, int tickInMS, int slotSize)
        {
            this.index = index;
            this.tickInMS = tickInMS;
            this.slotSize = slotSize;

            slotArr = new List<TimerTask>[slotSize];
        }

        internal bool AddTimerTask(TimerTask task, ref int slotIndex, ref int taskListIndex)
        {
            if (task.remainingInMS >= slotSize * tickInMS)
            {
                slotIndex = -1;
                taskListIndex = -1;
                return false;
            }

            int targetSlot = task.remainingInMS / tickInMS;
            if (targetSlot == 0)
            {
                targetSlot = 1;
                task.remainingInMS = 0;
            }
            else
            {
                task.remainingInMS = task.remainingInMS % tickInMS;
            }

            slotIndex = currentSlotIndex + targetSlot;
            slotIndex = slotIndex % slotSize;
            if (slotArr[slotIndex] == null)
            {
                slotArr[slotIndex] = new List<TimerTask>();
            }
            taskListIndex = slotArr[slotIndex].Count;
            slotArr[slotIndex].Add(task);

            return true;
        }

        internal bool RemoveTimerTask(int slotIndex, int taskListIndex)
        {
            List<TimerTask> taskList = slotArr[slotIndex];
            if (taskList != null && taskListIndex >= 0 && taskListIndex < taskList.Count)
            {
                taskList.RemoveAt(taskListIndex);
                return true;
            }
            return false;
        }

        internal void DoTimerTurn(int turnNum)
        {
            for (int i = 0; i < turnNum; i++)
            {
                currentSlotIndex++;
                if (currentSlotIndex == slotSize)
                {
                    currentSlotIndex = 0;
                    if (wheelOutEvent != null)
                    {
                        wheelOutEvent(index);
                    }
                }
                if (slotArr[currentSlotIndex] != null)
                {
                    willTriggerTaskList.AddRange(slotArr[currentSlotIndex]);
                    slotArr[currentSlotIndex].Clear();
                }
            }

            if (willTriggerTaskList.Count > 0)
            {
                if (wheelTriggerEvent != null)
                {
                    wheelTriggerEvent(index, willTriggerTaskList);
                }
                willTriggerTaskList.Clear();
            }
        }
    }
}