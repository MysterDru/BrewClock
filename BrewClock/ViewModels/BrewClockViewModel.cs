using System;
using PropertyChanged;
using System.Windows.Input;
using Xamarin.Forms;
#if __ANDROID__
using Android.OS;
#elif __IOS__
using Foundation;
#endif

namespace BrewClock
{
    [ImplementPropertyChanged]
    public class BrewClockViewModel
    {
        // default the event handler so we don't have to do a null check
        public event EventHandler BrewCompleted = delegate { };

        private ICommand changeBrewingStateCommand;
        private IBrewClockTimer brewCountTimer;

        /// <summary>
        /// Identifies the title of the page
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Identifies the curent selected brew time in minutes
        /// </summary>
        public int BrewTime { get; set; }

        /// <summary>
        /// Value displayed to the user based on the remaining brewtime
        /// </summary>
        public string BrewTimeDisplay
        {
            get
            {
                if (IsBrewing)
                {
                    return string.Format("{0}s", BrewCountDown);
                }
                else
                {
                    return string.Format("{0}m", BrewTime);
                }
            }
        }

        /// <summary>
        /// Identifies the seconds remaining before the brew is completed
        /// </summary>
        public long BrewCountDown { get; private set; }

        /// <summary>
        /// Total number of brews the user has made
        /// </summary>
        public int BrewCount { get; set; }

        /// <summary>
        /// Identifies the minimum time allowed for brewing
        /// </summary>
        public int MinimumBrewTime { get; private set; }

        /// <summary>
        /// Identifies the maximum time allowed for brewing
        /// </summary>
        public int MaximumBrewTime { get; private set; }

        /// <summary>
        /// Identifies if the brew timer is running
        /// </summary>
        public bool IsBrewing { get; set; }

        /// <summary>
        /// Identifies the text value that should be displayed in the button text.
        /// </summary>
        public string StartDisplay { get; set; }

        /// <summary>
        /// The command that should be executed in order to start or stop the brew timer
        /// </summary>
        public ICommand ChangeBrewingStateCommand
        {
            get
            {
                return changeBrewingStateCommand ?? (changeBrewingStateCommand = new Command(() =>
                {
                    if (this.IsBrewing)
                    {
                        this.StopBrewing();
                    }
                    else
                    {
                        this.StartBrewing();
                    }
                }));
            }
        }

        public BrewClockViewModel()
        {
            this.Title = "Brew Clock";
            this.BrewTime = 3;
            this.BrewCountDown = 0;
            this.MinimumBrewTime = 1;
            this.MaximumBrewTime = 10;
            this.IsBrewing = false;
            this.StartDisplay = "Start";
        }

        private void StartBrewing()
        {
            var secs = BrewTime * 60 * 1000;

#if __ANDROID__
            this.brewCountTimer = new BrewClockTimer(secs, 1000);
#elif __IOS__
            this.brewCountTimer = new BrewClockTimer(secs, 1000);
#endif
            this.BrewCountDown = secs / 1000;

            this.brewCountTimer.TickChanged += ((object sender, EventArgs<long> e) =>
            {
                BrewCountDown = (e.Value / 1000);
            });

            this.brewCountTimer.Finished += ((object sender, EventArgs e) =>
            {
                IsBrewing = false;
                BrewCount += 1;
                StartDisplay = "Start";

                this.BrewCompleted(this, new EventArgs());
            });

            this.brewCountTimer.StartTimer();
            this.StartDisplay = "Stop";

            this.IsBrewing = true;
        }

        private void StopBrewing()
        {
            if (this.brewCountTimer != null)
            {
                this.brewCountTimer.CancelTimer();
            }

            this.IsBrewing = false;
            this.StartDisplay = "Start";
        }

        /// <summary>
        /// Interface for abstracting timer logic to each platform
        /// </summary>
        private interface IBrewClockTimer
        {
            event EventHandler<EventArgs<long>> TickChanged;
            event EventHandler Finished;

            void CancelTimer();
            void StartTimer();
        }

#if __ANDROID__
        private class BrewClockTimer : CountDownTimer, IBrewClockTimer
        {
            public event EventHandler Finished = delegate { };

            public event EventHandler<EventArgs<long>> TickChanged = delegate { };

            public BrewClockTimer(long millisInFuture, long countDownInterval)
                : base(millisInFuture, countDownInterval)
            {
            }

            public override void OnTick(long millisUntilFinished)
            {
                this.TickChanged(this, new EventArgs<long>(millisUntilFinished));
            }

            public override void OnFinish()
            {
                this.Finished(this, new EventArgs());
            }

            public void CancelTimer()
            {
                this.Cancel();
            }

            public void StartTimer()
            {
                this.Start();
            }
        }
#elif __IOS__
        private class BrewClockTimer : IBrewClockTimer
        {
            public event EventHandler Finished = delegate { };

            public event EventHandler<EventArgs<long>> TickChanged = delegate { };

            private NSTimer timer;
            private long millisInFuture, countDownInterval, hours, minutes, seconds, secondsLeft;

            public BrewClockTimer(long millisInFuture, long countDownInterval)
            {
                this.millisInFuture = millisInFuture;
                this.countDownInterval = countDownInterval;
            }

            public void CancelTimer()
            {
                this.timer.Invalidate();
            }

            public void StartTimer()
            {
                this.secondsLeft = this.millisInFuture / 1000;

                this.timer = NSTimer.CreateRepeatingScheduledTimer(this.countDownInterval / 1000, this.UpdateCounter);
            }

            private void UpdateCounter(NSTimer theTimer)
            {
                if (this.secondsLeft > 0)
                {
                    this.secondsLeft--;
                    this.hours = secondsLeft / 3600;
                    this.minutes = (secondsLeft % 3600) / 60;
                    this.seconds = (secondsLeft % 3600) % 60;

                    // expexted value is in milliseconds
                    this.TickChanged(this, new EventArgs<long>(this.secondsLeft * 1000));
                }
                else
                {
                    this.Finished(this, new EventArgs());
                    this.CancelTimer();

                    this.secondsLeft = this.millisInFuture / 1000;
                }
            }
        }
#endif
    }
}