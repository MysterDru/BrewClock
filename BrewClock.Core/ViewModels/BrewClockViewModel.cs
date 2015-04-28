using System;
using PropertyChanged;
using System.Windows.Input;

namespace BrewClock
{
    [ImplementPropertyChanged]
    public class BrewClockViewModel : BaseViewModel
    {
        // default the event handler so we don't have to do a null check
        public event EventHandler BrewCompleted = delegate { };

        private IApp app;
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
            get { return this.app.TotalBrewCount; }
            set { this.app.TotalBrewCount = value; }
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
                return changeBrewingStateCommand ?? (changeBrewingStateCommand = new RelayCommand((object value) =>
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
            this.app = XLabs.Ioc.Resolver.Resolve<IApp>();

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

            // resolve and initialize the timer
            this.brewCountTimer = XLabs.Ioc.Resolver.Resolve<ICountDownTimer>();
            this.brewCountTimer.Initialize(secs, 1000);

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
    }
}