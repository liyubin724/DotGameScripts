using Dot.Core.Generic;
using Dot.Core.Util;
using Dot.Pool;
using System;
using System.Collections.Generic;


namespace Dot.Timer
{
    /// <summary>
    /// 
    /// </summary>
    public class TimerTaskInfo
    {
        internal long taskID = -1;
        internal int wheelIndex = -1;
        internal int wheelSlotIndex = -1;

        internal TimerTaskInfo()
        {
        }

        internal bool IsValid()
        {
            return taskID > 0 && wheelIndex > 0 && wheelSlotIndex > 0;
        }

        internal void Clear()
        {
            taskID = -1;
            wheelIndex = -1;
            wheelSlotIndex = -1;
        }

        public override string ToString()
        {
            return $"TimerTaskInfo:{{taskID = {taskID},wheelIndex = {wheelIndex},wheelSlotIndex = {wheelSlotIndex}}}";
        }
    }

    /// <summary>
    /// 多层时间轮
    /// </summary>
    internal sealed class HierarchicalTimerWheel
    {
        private UniqueID idCreator = new UniqueID();
        private ObjectPool<TimerTask> taskPool = new ObjectPool<TimerTask>();

        private TimerWheel[] wheelArr = null;
        private Dictionary<long, TimerTaskInfo> taskInfoDic = new Dictionary<long, TimerTaskInfo>();

        private int lapseTimeInMS = 0;

       /// <summary>
       /// 初始化多层时间轮，目前默认生成5层
       /// </summary>
        internal HierarchicalTimerWheel()
        {
            wheelArr = new TimerWheel[5];
            wheelArr[0] = new TimerWheel(0, 50, 20);
            wheelArr[1] = new TimerWheel(1, wheelArr[0].TotalTickInMS, 60);
            wheelArr[2] = new TimerWheel(2, wheelArr[1].TotalTickInMS, 60);
            wheelArr[3] = new TimerWheel(3, wheelArr[2].TotalTickInMS, 24);
            wheelArr[4] = new TimerWheel(4, wheelArr[3].TotalTickInMS, 30);

            for (int i = 0; i < wheelArr.Length; i++)
            {
                wheelArr[i].slotTriggerEvent = OnWheelSlotTrigger;
                wheelArr[i].turnOnceEvent = OnWheelTurnOnce;
            }
        }

        internal TimerTaskInfo AddTimerTask(float intervalInSec,
                                                float totalInSec,
                                                TimerCallback intervalCallback,
                                                TimerCallback endCallback,
                                                object callbackData)
        {
            TimerTask task = taskPool.Get();
            task.SetData(idCreator.NextID, intervalInSec, totalInSec, intervalCallback, endCallback, callbackData);

            TimerTaskInfo taskInfo = new TimerTaskInfo();
            taskInfo.taskID = task.ID;

            if(AddTimerTask(task,taskInfo))
            {
                return taskInfo;
            }
            throw new Exception($"HierarchicalTimerWheel::AddTimerTask->Add Failed");
        }

        private bool AddTimerTask(TimerTask task, TimerTaskInfo taskInfo)
        {
            for (int i = 0; i < wheelArr.Length; i++)
            {
                if(wheelArr[i].TotalTickInMS>task.RemainingInMS)
                {
                    int slotIndex = wheelArr[i].AddTimerTask(task);
                    taskInfo.wheelIndex = i;
                    taskInfo.wheelSlotIndex = slotIndex;
                    return true;
                }
            }
            
            return false;
        }

        internal bool RemoveTimerTask(TimerTaskInfo taskInfo)
        {
            if(taskInfo == null|| !taskInfo.IsValid())
            {
                return false;
            }

            if(!taskInfoDic.ContainsKey(taskInfo.taskID))
            {
                return false;
            }

            if (taskInfo == null || taskInfo.wheelIndex < 0 || taskInfo.wheelSlotIndex < 0 || taskInfo.taskID < 0)
            {
                return false;
            }

            taskInfoDic.Remove(taskInfo.taskID);

            long taskID = taskInfo.taskID;
            int wheelIndex = taskInfo.wheelIndex;
            int wheelSlotIndex = taskInfo.wheelSlotIndex;
            taskInfo.Clear();

            return wheelArr[wheelIndex].RemoveTimerTask(wheelSlotIndex, taskID);
        }

        internal void OnUpdate(float deltaTime)
        {
            if(taskInfoDic.Count == 0)
            {
                return;
            }
            lapseTimeInMS += MathUtil.CeilToInt (deltaTime * 1000);

            TimerWheel wheel = wheelArr[0];
            int turnNum = lapseTimeInMS / wheel.TickInMS;
            if(turnNum>0)
            {
                wheel.DoTimerTurn(turnNum);
                lapseTimeInMS = lapseTimeInMS % wheel.TickInMS;
            }
        }

        private void OnWheelTurnOnce(int index)
        {
            if (index >= 0 && index < wheelArr.Length)
            {
                wheelArr[index + 1].DoTimerTurn(1);
            }
        }

        private void OnWheelSlotTrigger(int index, TimerTask[] tasks)
        {
            if(tasks == null || tasks.Length>0)
            {
                return;
            }

            for(int i =0;i<tasks.Length;++i)
            {
                TimerTask task = tasks[i];
                if(task == null)
                {
                    continue;
                }
                if (!taskInfoDic.TryGetValue(task.ID, out TimerTaskInfo taskInfo))
                {
                    continue;
                }
                if(index == 0)
                {
                    task.OnTaskInterval();
                    if(task.IsEnd())
                    {
                        taskInfoDic.Remove(task.ID);
                        taskInfo.Clear();

                        taskPool.Release(task);
                        task.OnTaskEnd();
                    }else
                    {
                        task.ResetTask();
                        AddTimerTask(task, taskInfo);
                    }
                }else
                {
                    AddTimerTask(task, taskInfo);
                }
            }
        }

        internal void Clear()
        {
            if (taskInfoDic.Count > 0)
            {
                List<long> keys = new List<long>(taskInfoDic.Keys);
                foreach(var key in keys)
                {
                    RemoveTimerTask(taskInfoDic[key]);
                }
            }
            lapseTimeInMS = 0;
        }
    }
}


