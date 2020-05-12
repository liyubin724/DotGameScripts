using Dot.Proxy;

namespace Dot.Timer
{
    public delegate void TimerCallback(object obj);

    /// <summary>
    /// 定时器的管理器
    /// </summary>
    public class TimerManager : Singleton<TimerManager>
    {
        //多层时间轮
        private HierarchicalTimerWheel hTimerWheel = null;
        //是否暂停
        private bool isPause = false;

        protected override void DoInit()
        {
            hTimerWheel = new HierarchicalTimerWheel();
            UpdateProxy.GetInstance().DoUpdateHandle += DoUpdate;
        }

        private void DoUpdate(float deltaTime)
        {
            if (!isPause && hTimerWheel != null)
            {
                hTimerWheel.OnUpdate(deltaTime);
            }
        }
        /// <summary>
        /// 暂停时间轮
        /// </summary>
        public void Pause()
        {
            isPause = true;
        }
        /// <summary>
        /// 恢复时间轮
        /// </summary>
        public void Resume()
        {
            isPause = false;
        }
        /// <summary>
        /// 添加定时器
        /// </summary>
        /// <param name="intervalInSec">触发时间间隔，以秒计</param>
        /// <param name="totalInSec">定时总时长，以秒计</param>
        /// <param name="intervalCallback">间隔触发回调</param>
        /// <param name="endCallback">结束时回调</param>
        /// <param name="userData">自定义参数</param>
        /// <returns></returns>
        public TimerTaskInfo AddTimer(float intervalInSec,
                                                float totalInSec,
                                                TimerCallback intervalCallback,
                                                TimerCallback endCallback,
                                                object userData)
        {
            if (hTimerWheel == null) return null;
            return hTimerWheel.AddTimerTask(intervalInSec, totalInSec, intervalCallback, endCallback, userData);
        }

        /// <summary>
        /// 添加定时器，默认不会自动中止，会按指定的时间不断触发
        /// </summary>
        /// <param name="intervalInSec">触发间隔</param>
        /// <param name="intervalCallback">触发回调</param>
        /// <param name="userData">自定义参数</param>
        /// <returns></returns>
        public TimerTaskInfo AddIntervalTimer(float intervalInSec, 
                                                                    TimerCallback intervalCallback, 
                                                                    object userData = null)
        {
            return AddTimer(intervalInSec, 0f, intervalCallback, null, userData);
        }
        /// <summary>
        /// 添加定时器，默认一次性执行，当执行结束后会自动删除定时器
        /// </summary>
        /// <param name="totalInSec">时长，以秒计</param>
        /// <param name="endCallback">达到指定时长后回调</param>
        /// <param name="userData">自定义参数</param>
        /// <returns></returns>
        public TimerTaskInfo AddEndTimer(float totalInSec, 
                                                                TimerCallback endCallback,
                                                                object userData = null)
        {
            return AddTimer(totalInSec, totalInSec, null, endCallback, userData);
        }
        /// <summary>
        /// 删除定时器
        /// </summary>
        /// <param name="taskInfo"></param>
        /// <returns></returns>
        public bool RemoveTimer(TimerTaskInfo taskInfo)
        {
            if (hTimerWheel != null)
            {
                return hTimerWheel.RemoveTimerTask(taskInfo);
            }
            return false;
        }

        public override void DoDispose()
        {
            UpdateProxy.GetInstance().DoUpdateHandle -= DoUpdate;

            hTimerWheel?.Clear();
            isPause = false;
            hTimerWheel = null;
        }
    }
}
