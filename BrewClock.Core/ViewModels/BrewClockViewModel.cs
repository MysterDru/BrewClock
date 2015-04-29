using System;
using PropertyChanged;
using System.Windows.Input;
using Xamarin.Forms;

namespace BrewClock
{
    [ImplementPropertyChanged]
    public class BrewClockViewModel
    {
        // default the event handler so we don't have to do a null check
        /// <summary>
        /// Let the view know when the brew is completed so they can alert the user
        /// </summary>
        public event EventHandler BrewCompleted = delegate { };

        private ICommand changeBrewingStateCommand;
        private ICountDownTimer brewCountTimer;

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
        // proxied to the App class
        public int BrewCount
        {
            get { return App.Instance.TotalBrewCount; }
            set { App.Instance.TotalBrewCount = value; }
        }

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
            var millisecs = BrewTime * 60 * 1000;

            // resolve and initialize the timer
            this.brewCountTimer = DependencyService.Get<ICountDownTimer>();
            this.brewCountTimer.Initialize(millisecs, 1000);

            this.BrewCountDown = millisecs / 1000;

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
    }
}