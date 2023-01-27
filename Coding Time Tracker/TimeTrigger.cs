using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Coding_Time_Tracker
{
    /// <summary>
    /// Provides a disposable object that will trigger periodically with a given delay.
    /// </summary>
    public class TimeTrigger : IDisposable
    {
        /// <summary>
        /// The time of the next trigger.
        /// </summary>
        public DateTime TriggerTime
        {
            get { return _triggerTime; }
        }


        /// <summary>
        /// The delay between triggers.
        /// </summary>
        public TimeSpan TriggerDelay
        {
            get { return _triggerDelay; }
        }


        /// <summary>
        /// Initiator
        /// </summary>
        /// <param name="seconds">The ammout of seconds to delay.</param>
        public TimeTrigger(int seconds)
        {
            _triggerDelay = TimeSpan.FromSeconds(seconds);
            Start();
        }


        /// <summary>
        /// Initiator
        /// </summary>
        /// <param name="minutes">The ammout of minutes to delay.</param>
        /// <param name="seconds">The ammout of seconds to delay.</param>
        public TimeTrigger(int minutes, int seconds)
        {
            _triggerDelay = new TimeSpan(0, minutes, seconds);
            Start();
        }


        /// <summary>
        /// Initiator
        /// </summary>
        /// <param name="hours">The ammout of hours to delay.</param>
        /// <param name="minutes">The ammout of minutes to delay.</param>
        /// <param name="seconds">The ammout of seconds to delay.</param>
        public TimeTrigger(int hours, int minutes, int seconds)
        {
            _triggerDelay = new TimeSpan(hours, minutes, seconds);
            Start();
        }


        /// <summary>
        /// Initiator
        /// </summary>
        /// <param name="triggerDelay">Delay in TimeSpan format.</param>
        public TimeTrigger(TimeSpan triggerDelay)
        {
            _triggerDelay = triggerDelay;
            Start();
        }

        /// <summary>
        /// Stops and disposes the object.
        /// </summary>
        public void Stop()
        {
            Dispose();
        }

        public void Dispose()
        {
            _tokenSource?.Cancel();
            _tokenSource?.Dispose();
            _tokenSource = null;
            _runningTask?.Dispose();
            _runningTask = null;
        }

        /// <summary>
        /// Disposes the object when out of scope.
        /// </summary>
        ~TimeTrigger() => Dispose();


        /// <summary>
        /// Triggers periodically with the specified delay.
        /// </summary>
        public event Action? Triggered;

        /// <summary>
        /// Handler for Triggered event.
        /// </summary>
        protected virtual void OnTriggered()
        {
            Triggered?.Invoke();
        }

        private void Start()
        {
            if (_triggerDelay < TimeSpan.Zero)
            {
                throw new InvalidDataException();
            }

            if (_runningTask != null)
            {
                throw new Exception();
            }

            _tokenSource = new CancellationTokenSource();

            SetTriggerTime();

            _runningTask = Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    await Task.Delay(1000, _tokenSource.Token);
                    if (_triggerTime <= DateTime.Now)
                    {
                        OnTriggered();
                        SetTriggerTime();
                    }
                }
            }, _tokenSource.Token);
        }


        private void SetTriggerTime()
        {
            _triggerTime = DateTime.Now + _triggerDelay;
        }


        private DateTime _triggerTime;

        private TimeSpan _triggerDelay;

        private CancellationTokenSource? _tokenSource = null;

        private Task? _runningTask = null;
    }
}
