using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrewClock
{
    /// <summary>
    /// Interface for abstracting timer logic to each platform
    /// ICountDownTimer: this will be resolved and implemented 
    ///     within each platform projects
    /// </summary>
    public interface ICountDownTimer
    {
        /// <summary>
        /// Event handler that is invoked everytime the value of the timer is changed
        /// </summary>
        event EventHandler<EventArgs<long>> TickChanged;

        /// <summary>
        /// Event handler that is invoked once the timer has finished running
        /// </summary>
        event EventHandler Finished;

        /// <summary>
        /// Cancels the timer. Can only be called if Initialize() has been invoked
        /// </summary>
        void CancelTimer();

        /// <summary>
        /// Starts the timer. Can only be called if Initialize() has been invoked
        /// </summary>
        void StartTimer();

        /// <summary>
        /// Initializes a new timer. Afterwhich, the timer can be started
        /// </summary>
        /// <param name="millisInFuture">When to start the timer</param>
        /// <param name="countDownInterval">the interval the timer should countdown in</param>
        void Initialize(long millisInFuture, long countDownInterval);
    }
}