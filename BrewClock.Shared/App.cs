using System;

using Xamarin.Forms;
using XLabs.Ioc.SimpleInjectorContainer;

namespace BrewClock
{
    public class App : Application, IApp
    {
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
            
            var container = new SimpleInjectorContainer();

            container.Register<IApp>(this);

            #if __ANDROID__
            container.Register<ICountDownTimer, BrewClock.Droid.CountDownTimer>();
            #elif __IOS__
            container.Register<ICountDownTimer, BrewClock.iOS.CountDownTimer>();
            #endif

            XLabs.Ioc.Resolver.SetResolver(container.GetResolver());

            // The root page of your application
            MainPage = new NavigationPage(new BrewClockPage())
            {
                BarBackgroundColor = Color.White,
                BarTextColor = Color.Black
            };
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

