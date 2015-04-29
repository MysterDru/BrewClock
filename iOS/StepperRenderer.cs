using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

// tell the Xamarin.Forms framework we want to use our custom renderer
// when creating the UIStepper control
//[assembly: ExportRenderer(typeof(Stepper), typeof(BrewClock.iOS.BrewclockStepperRenderer))]

namespace BrewClock.iOS
{
    public class BrewclockStepperRenderer 
        : Xamarin.Forms.Platform.iOS.StepperRenderer
    {
        /// <summary>
        /// Called everytime the Stepper element changes.
        /// Usually this is when the stepper is being created & disposed
        /// </summary>
        protected override void OnElementChanged(Xamarin.Forms.Platform.iOS.ElementChangedEventArgs<Xamarin.Forms.Stepper> e)
        {
            base.OnElementChanged(e);

            // perform the initial setup
            if (Control != null)
            {
                this.Control.TintColor = Color.Red.ToUIColor();
            }
        }

        /// <summary>
        /// Called everytime a property/value on the "Stepper" changes
        /// We could refresh our UIStepper properties here if we had custom
        /// Stepper class
        /// </summary>
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }
    }
}