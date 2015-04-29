using System;

// mark the CountDownTimer class as a dependancy. Once done, it can be referenced
// using DependencyService.Get()
[assembly: Xamarin.Forms.Dependency(typeof(BrewClock.Droid.CountDownTimer))]

namespace BrewClock.Droid
{
    public class CountDownTimer : Java.Lang.Object, ICountDownTimer
    {
        public event EventHandler Finished = delegate { };
        public event EventHandler<EventArgs<long>> TickChanged = delegate { };

        private InternalCountDownTimer timer;

        public void Initialize(long millisInFuture, long countDownInterval)
        {
            // if the timer was previouslly initialize, remove the event handlers
            this.RemoveHandlers();

            this.timer = new InternalCountDownTimer(millisInFuture, countDownInterval);
            this.timer.Finished += this.HandleFinished;
            this.timer.TickChanged += this.HandleTickChanged;
        }

        public void CancelTimer()
        {
            this.timer.Cancel();
        }

        public void StartTimer()
        {
            this.timer.Start();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.RemoveHandlers();
            }

            base.Dispose(disposing);
        }

        // re-usable method for removing event handlers
        private void RemoveHandlers()
        {
            if (this.timer != null)
            {
                this.timer.Finished -= this.HandleFinished;
                this.timer.TickChanged -= this.HandleTickChanged;
            }
        }

        private void HandleFinished(object sender, EventArgs args)
        {
            this.Finished(this, args);
        }

        private void HandleTickChanged(object sender,EventArgs<long> args)
        {
            this.TickChanged(this, args);
        }

        // we need to create a subsclass of Android.OS.CountDownTimer since it is 
        // abstract. This gives us access to the OnTick() & OnFinish() methods
        private class InternalCountDownTimer : Android.OS.CountDownTimer
        {
            public event EventHandler Finished = delegate { };

            public event EventHandler<EventArgs<long>> TickChanged = delegate { };

            public InternalCountDownTimer(long millisInFuture, long countDownInterval)
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
        }
    }
}