using System;

namespace DotEngine.Framework.Services
{
    public interface ITimerTaskInfo
    {
    }

    public interface ITimerService : IService, IUpdate
    {
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
