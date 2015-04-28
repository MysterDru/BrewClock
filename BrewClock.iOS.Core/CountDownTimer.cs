using System;
using Foundation;

namespace BrewClock.iOS
{
    public class CountDownTimer : ICountDownTimer
    {
        public event EventHandler Finished = delegate { };
        public event EventHandler<EventArgs<long>> TickChanged = delegate { };

        private NSTimer timer;
        private long millisInFuture, countDownInterval, hours, minutes, seconds, secondsLeft;


        public void Initialize(long millisInFuture, long countDownInterval)
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
}