using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Coding_Time_Tracker
{
    public class TimeTrigger : IDisposable
    {
        public CancellationTokenSource? TokenSource { get; private set; } = null;
        public Task? RunningTask { get; private set; } = null;

        private TimeSpan _triggerAfter;

        public TimeTrigger(int seconds)
        {
            _triggerAfter = TimeSpan.FromSeconds(seconds);
            Start();
        }
        public TimeTrigger(int minutes, int seconds)
        {
            _triggerAfter = new TimeSpan(0, minutes, seconds);
            Start();
        }
        public TimeTrigger(int hours, int minutes, int seconds)
        {
            _triggerAfter = new TimeSpan(hours, minutes, seconds);
            Start();
        }
        public TimeTrigger(TimeSpan triggerAfter)
        {
            _triggerAfter = triggerAfter;
            Start();
        }


        private void Start()
        {
            if (_triggerAfter < TimeSpan.Zero)
            {
                throw new InvalidDataException();
            }

            if (RunningTask != null)
            {
                throw new Exception();
            }

            DateTime triggerTime = DateTime.Now + _triggerAfter;
            TokenSource = new CancellationTokenSource();

            RunningTask = Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    if (triggerTime <= DateTime.Now)
                    {
                        OnTriggered();
                        Dispose();
                        return;
                    }
                    await Task.Delay(1000, TokenSource.Token);
                }
            }, TokenSource.Token);
        }
        public void Cancel()
        {
            Dispose();
        }

        protected virtual void OnTriggered()
        {
            Triggered?.Invoke();
        }

        public void Dispose()
        {
            TokenSource?.Cancel();
            TokenSource?.Dispose();
            TokenSource = null;
            RunningTask?.Dispose();
            RunningTask = null;
        }
        ~TimeTrigger() => Dispose();

        public event Action? Triggered;
    }
}
