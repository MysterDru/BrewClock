using System;
using PropertyChanged;
using System.Windows.Input;
using Xamarin.Forms;
using XLabs;
#if __IOS__
using Foundation;
#endif

#if __ANDROID__
using Android.OS;
#endif

namespace BrewClock
{
	[ImplementPropertyChanged]
	public class BrewClockViewModel
	{
		private ICommand startBrewCommand;
		private ICommand addBrewTimeCommand;
		private ICommand decreaseBrewTimeCommand;
		private int brewTime;
		private BrewClockTimer brewCountTimer;

		public string Title { get; private set; } = "Brew Clock";

		public int BrewTime { get; set; } = 3;

		public long BrewCountDown { get; private set; } = 0;

		public int BrewCount { get; set; } = 0;

		public int MinimumBrewCount { get; private set; } = 1;

		public bool IsBrewing { get; set; } = false;

		public string BrewTimeDisplay {
			get {
				if (IsBrewing) {
					return string.Format ("{0}s", BrewCountDown);
				}
				else if(BrewTime == 0 && !IsBrewing) {
					return "Brew Up!";
				}
				else {
					return string.Format ("{0}m", BrewTime);
				} 
			}
		}

		public string StartDisplay { get; set; } = "Start";

		public ICommand StartBrewCommand {
			get 
			{
				return startBrewCommand ?? (startBrewCommand = new Command (() => {
					if (IsBrewing)
						StopBrewing ();
					else
						StartBrewing ();
				}));
			}
		}

//		public ICommand AddBrewTimeCommand {
//			get {
//				return addBrewTimeCommand ?? (addBrewTimeCommand = new Command (() => {
//					BrewTime += 1;
//				}));
//			}
//		}
//
//		public ICommand DecreaseBrewTimeCommand {
//			get {
//				return decreaseBrewTimeCommand?? (decreaseBrewTimeCommand = new Command(() => {
//					BrewTime -= 1;
//				}));
//			}
//		}


		public BrewClockViewModel ()
		{
		}

		private void StartBrewing()
		{
			var secs = BrewTime * 60 * 1000;

			brewCountTimer = new BrewClockTimer (secs, 1000);
			BrewCountDown = secs / 1000;

			brewCountTimer.TickChanged	= ((object sender, EventArgs<long> e) => {
				BrewCountDown = (e.Value / 1000);
//				BrewTimeDisplay = String.Format("{0}s", e.Value / 1000);
			});

			brewCountTimer.Finished = ((object sender, EventArgs e) => {
				IsBrewing = false;
				BrewCount += 1;
				StartDisplay = "Start";
			});

			brewCountTimer.Start ();
			StartDisplay = "Stop";

			IsBrewing = true;
		}

		private void StopBrewing()
		{
			if (brewCountTimer != null) {
				brewCountTimer.Cancel ();
			}

			IsBrewing = false;
			StartDisplay = "Start";
		}

		#if __ANDROID__
		private class BrewClockTimer : CountDownTimer
		{
			public EventHandler Finished = delegate { };

			public EventHandler<EventArgs<long>> TickChanged = delegate { };

			public BrewClockTimer (long millisInFuture, long countDownInterval) : base(millisInFuture, countDownInterval) {
			}

			public override void OnTick (long millisUntilFinished)
			{
				TickChanged(this, new EventArgs<long>(millisUntilFinished));
			}

			public override void OnFinish ()
			{
				Finished (this, new EventArgs ());
			}
		}
		#endif

		#if _IOS_
		private class BrewClockTimer {

			public EventHandler Finished = delegate { };

			public EventHandler<EventArgs<long>> TickChanged = delegate { };

			public BrewClockTimer(long millisInFuture, long countDownInterval)
			{

				var timer =	NSTimer.CreateScheduledTimer(millisInFuture / 1000, HandleAction);

			}

		}
		#endif
	}
}