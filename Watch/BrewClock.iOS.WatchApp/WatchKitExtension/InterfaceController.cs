using System;

using WatchKit;
using Foundation;
using BrewClock;

namespace WatchKitExtension
{
    public partial class InterfaceController : BaseInterfaceController
    {
        private BrewClockViewModel viewModel;

        public InterfaceController(IntPtr handle)
            : base(handle)
        {
        }

        public override void Awake(NSObject context)
        {
            base.Awake(context);

            this.viewModel = new BrewClockViewModel();

            // Configure interface objects here.
            Console.WriteLine("{0} awake with context", this);
        }

        public override void WillActivate()
        {
            // This method is called when the watch view controller is about to be visible to the user.
            Console.WriteLine("{0} will activate", this);

            viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        public override void DidDeactivate()
        {
            // This method is called when the watch view controller is no longer visible to the user.
            Console.WriteLine("{0} did deactivate", this);

            viewModel.PropertyChanged -= ViewModel_PropertyChanged;
        }

        partial void sliderAction(float value)
        {
            this.viewModel.BrewTime = (int)value;
        }

        partial void StartButton_Activated(WKInterfaceButton sender)
        {
            if(this.viewModel.ChangeBrewingStateCommand.CanExecute(null))
            {
                this.viewModel.ChangeBrewingStateCommand.Execute(null);
            }
        }

        private void ViewModel_PropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.RefreshDataBindings();
        }

        private void RefreshDataBindings()
        {
            this.BrewsLabel.SetText("Brews: " + this.viewModel.BrewCount);

            this.BrewTimeLabel.SetText(this.viewModel.BrewTimeDisplay);
            this.StartButton.SetTitle(this.viewModel.StartDisplay);
            this.BrewTimeSlider.SetNumberOfSteps(1);//this.viewModel.MaximumBrewTime - this.viewModel.MinimumBrewTime);
            this.BrewTimeSlider.SetValue(this.viewModel.BrewTime);
        }
    }
}