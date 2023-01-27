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
        public DateTime TriggerTime
        {
            get 
            { 
                return DateTime.Now + _triggerAfter; 
            }
        }

        private CancellationTokenSource? _tokenSource = null;

        private Task? _runningTask = null;

        private TimeSpan _triggerAfter = TimeSpan.Zero;

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

            if (_runningTask != null)
            {
                throw new Exception();
            }

            _tokenSource = new CancellationTokenSource();

            _runningTask = Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    if (TriggerTime <= DateTime.Now)
                    {
                        OnTriggered();
                        Dispose();
                        return;
                    }
                    await Task.Delay(1000, _tokenSource.Token);
                }
            }, _tokenSource.Token);
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
            _tokenSource?.Cancel();
            _tokenSource?.Dispose();
            _tokenSource = null;
            _runningTask?.Dispose();
            _runningTask = null;
        }
        ~TimeTrigger() => Dispose();

        public event Action? Triggered;
    }
}
