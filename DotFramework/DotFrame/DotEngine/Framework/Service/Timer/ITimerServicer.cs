using System;

namespace DotEngine.Framework.Service
{
    public interface ITimerTaskInfo
    {
    }

    public interface ITimerServicer : IServicer
    {
        void DoUpdate(float deltaTime);
        void Pause();
        void Resume();
        ITimerTaskInfo AddTimer(float intervalInSec,
                                                float totalInSec,
                                                Action<object> intervalCallback,
                                                Action<object> endCallback,
                                                object userData);
        ITimerTaskInfo AddIntervalTimer(float intervalInSec,
                                                                    Action<object> intervalCallback,
                                                                    object userData = null);
        ITimerTaskInfo AddEndTimer(float totalInSec,
                                                                Action<object> endCallback,
                                                                object userData = null);
        bool RemoveTimer(ITimerTaskInfo taskInfo);
    }
}
