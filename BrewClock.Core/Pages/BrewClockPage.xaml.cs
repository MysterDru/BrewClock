using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace BrewClock
{
	public partial class BrewClockPage : ContentPage
	{
		public BrewClockPage ()
		{
			this.BindingContext = new BrewClockViewModel ();
            
			InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // add the event handler for completed when the page becomes visible
            (this.BindingContext as BrewClockViewModel).BrewCompleted += BrewClockPage_BrewCompleted;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            // add the event handler for completed when the page is no longer visible
            (this.BindingContext as BrewClockViewModel).BrewCompleted -= BrewClockPage_BrewCompleted;
        }

        void BrewClockPage_BrewCompleted(object sender, EventArgs e)
        {
            this.DisplayAlert("Brew Up!", "Your delicious tea is finished brewing and ready to drink!", "Ok");
        }

        // this variation is async, and will display the alert on a background thread
        //async void BrewClockPage_BrewCompleted(object sender, EventArgs e)
        //{
        //    await this.DisplayAlert("Brew Up!", "Your delicious tea is finished brewing and ready to drink!", "Ok");
        //}
	}
}

