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
	}
}

