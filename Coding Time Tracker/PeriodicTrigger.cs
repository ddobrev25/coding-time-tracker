using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Coding_Time_Tracker
{
    /// <summary>
    /// Provides a disposable object that will trigger an event periodically with a given delay.
    /// </summary>
    public class PeriodicTrigger : IDisposable
    {
        /// <summary>
        /// The time of the next trigger.
        /// </summary>
        public DateTime TriggerTime { get; private set; }


        /// <summary>
        /// The delay between triggers.
        /// </summary>
        public TimeSpan TriggerDelay { get; private set; }


        /// <summary>
        /// Takes seconds as an argument for delay and initiates the timer.
        /// </summary>
        /// <param name="seconds">The ammout of seconds to delay.</param>
        /// <exception cref="ArgumentException"></exception>
        public PeriodicTrigger(int seconds)
        {
            TimeSpan triggerDelay = TimeSpan.FromSeconds(seconds);

            if(!IsDelayInCorrectFormat(triggerDelay))
            {
                throw new ArgumentException("Trigger delay value is not in the correct format.");
            }

            TriggerDelay = triggerDelay;
            _ = Start();
        }

        /// <summary>
        /// Takes minutes and seconds as arguments for delay and initiates the timer.
        /// </summary>
        /// <param name="minutes">The ammout of minutes to delay.</param>
        /// <param name="seconds">The ammout of seconds to delay.</param>
        /// <exception cref="ArgumentException"></exception>
        public PeriodicTrigger(int minutes, int seconds)
        {
            TimeSpan triggerDelay = new TimeSpan(0, minutes, seconds);

            if (!IsDelayInCorrectFormat(triggerDelay))
            {
                throw new ArgumentException("Trigger delay value is not in the correct format.");
            }

            TriggerDelay = triggerDelay;
            _ = Start();
        }

        /// <summary>
        /// Takes hours, minutes and seconds as arguments for delay and initiates the timer.
        /// </summary>
        /// <param name="hours">The ammout of hours to delay.</param>
        /// <param name="minutes">The ammout of minutes to delay.</param>
        /// <param name="seconds">The ammout of seconds to delay.</param>
        /// <exception cref="ArgumentException"></exception>
        public PeriodicTrigger(int hours, int minutes, int seconds)
        {
            TimeSpan triggerDelay = new TimeSpan(hours, minutes, seconds);

            if (!IsDelayInCorrectFormat(triggerDelay))
            {
                throw new ArgumentException("Trigger delay value is not in the correct format.");
            }

            TriggerDelay = triggerDelay;
            _ = Start();
        }


        /// <summary>
        /// Takes a TimeSpan object for delay and initiates the timer.
        /// </summary>
        /// <param name="triggerDelay">Delay in TimeSpan format.</param>
        /// <exception cref="ArgumentException"></exception>
        public PeriodicTrigger(TimeSpan triggerDelay)
        {
            if (!IsDelayInCorrectFormat(triggerDelay))
            {
                throw new ArgumentException("Trigger delay value is not in the correct format.");
            }

            TriggerDelay = triggerDelay;
            _ = Start();
        }

        /// <summary>
        /// Stops and disposes the object.
        /// </summary>
        public void Stop()
        {
            Dispose();
        }


        ///<inheritdoc/>
        public void Dispose()
        {
            _tokenSource?.Cancel();
            _tokenSource?.Dispose();
            _tokenSource = null;
        }

        /// <summary>
        /// Disposes the object when out of scope.
        /// </summary>
        ~PeriodicTrigger() => Dispose();


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



        /// <summary>
        /// Initiates the internal clock of the object.
        /// </summary>
        private async Task Start()
        {
            _tokenSource = new CancellationTokenSource();

            CalculateTriggerTime();

            while (true)
            {
                if (TriggerTime <= DateTime.Now)
                {
                    OnTriggered();
                    CalculateTriggerTime();
                }
                else
                {
                    await Task.Delay(1000, _tokenSource.Token);
                }
            }
        }



        /// <summary>
        /// Checks whether a given delay value is in the correct format.
        /// </summary>
        /// <param name="delay">The delay to check.</param>
        /// <returns>Whether a given delay value is in the correct format.</returns>
        private bool IsDelayInCorrectFormat(TimeSpan delay) => delay > TimeSpan.Zero;




        /// <summary>
        /// Calculates and sets the trigger time to the correct value compared to the current time and the trigger delay.
        /// </summary>
        private void CalculateTriggerTime() => TriggerTime = DateTime.Now + TriggerDelay;



        /// <summary>
        /// Token source that will provide a cancellation token.
        /// </summary>
        private CancellationTokenSource? _tokenSource = null;
    }
}
