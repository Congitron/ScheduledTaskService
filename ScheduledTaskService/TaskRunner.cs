using ScheduledTaskService.Tasks;
using System;

namespace ScheduledTaskService
{
    public class TaskRunner
    {
        private IScheduledTask _task;
        private System.Timers.Timer _timer = new System.Timers.Timer();

        public TaskRunner(IScheduledTask task) { _task = task; }
        public TaskRunner() { }

        public IScheduledTask Task { get { return _task; } set { _task = value; } }

        public void Start()
        {
            if (_task == null) { throw new Exception("Scheduled task cannot be null!"); }
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            OnTimer(null, null);
        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            DateTime now = DateTime.Now;
            double nowInSeconds = now.TimeOfDay.TotalSeconds;
            double runTime = _task.RunTime;
            double open = _task.RunTime - 300; // 5 minute window on either side of the targeted runtime
            double close = _task.RunTime + 300;

            if (open < 0)
            {
                runTime -= open;
                open -= open;
                close -= open;
                nowInSeconds -= open;
            }
            else if (close > 86400)
            {
                double offset = close - 86400;

                runTime -= offset;
                open -= offset;
                close -= offset;
                nowInSeconds -= offset;
            }

            if (nowInSeconds >= open && nowInSeconds <= close) { _task.RunTask(); }

            // Now wait for the next runtime
            now = DateTime.Now;
            nowInSeconds = now.TimeOfDay.TotalSeconds;
            double elapsed = nowInSeconds - _task.RunTime; // How long it took us to run this task / How long it's been since runtime
            double sleepTime = 86400 - elapsed;

            _timer.Stop();
            _timer.Interval = (int)sleepTime * 1000;
            _timer.Start();
        }
    }
}
