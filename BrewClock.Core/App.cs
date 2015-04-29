using System;

using Xamarin.Forms;

namespace BrewClock
{
    public class App : Application
    {
        // let's us get at the app staically
        public static App Instance
        {
            get { return Xamarin.Forms.Application.Current as App; }
        }

        /// <summary>
        /// Let's us track the total count across sessions of the app 
        /// </summary>
        public int TotalBrewCount
        {
            get
            {
                if (this.Properties.ContainsKey("TotalBrewCount"))
                {
                    return (int)this.Properties["TotalBrewCount"];
                }
                else { return 0; }
            }
            set
            {
                if (this.Properties.ContainsKey("TotalBrewCount"))
                {
                    this.Properties.Remove("TotalBrewCount");
                }

                this.Properties["TotalBrewCount"] = value;
            }
        }

        public App()
        {
            // The root page of your application

            // Create main page stand alone
            MainPage = new BrewClockPage();

            // Create the main page with a navigation page
            //MainPage = new NavigationPage(new BrewClockPage())
            //{
            //    BarBackgroundColor = Color.White,
            //    BarTextColor = Color.Black
            //};
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

