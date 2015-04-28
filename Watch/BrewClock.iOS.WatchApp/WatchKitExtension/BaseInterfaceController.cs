using System;
using WatchKit;
using BrewClock;
using XLabs.Ioc.SimpleInjectorContainer;

namespace WatchKitExtension
{
    public abstract class BaseInterfaceController : WKInterfaceController, IApp
    {
        #region IApp implementation
        public int TotalBrewCount  { get; set; }
        #endregion

        protected BaseInterfaceController(IntPtr handle)
            : base(handle)
        {
            var container = new SimpleInjectorContainer();

            container.Register<IApp>(this);
            container.Register<ICountDownTimer, BrewClock.iOS.CountDownTimer>();

            XLabs.Ioc.Resolver.SetResolver(container.GetResolver());
        } 
    }
}

