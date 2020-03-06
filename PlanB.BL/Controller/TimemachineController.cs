using PlanB.BL.Model;
using System;

namespace PlanB.BL.Controller
{
    /// <summary>
    /// Create new timer and convert minutes and seconds to hundredths.
    /// </summary>
    class TimemachineController
    {
        /// <summary>
        /// Create a new timer.
        /// </summary>
        private Timemachine Timer { get; }

        /// <summary>
        /// Convert minutes and seconds to hundredths
        /// </summary>
        public int Value
        {
            get
            {
                return Timer.Minutes * 6000 + Timer.Seconds * 100 + Timer.Hundredths;
            }
        }

        /// <summary>
        /// Create a new TimemachineController
        /// </summary>
        /// <param name="minutes"> minutes </param>
        /// <param name="seconds"> seconds </param>
        /// <param name="hundredths"> hundredths </param>
        public TimemachineController(int minutes, int seconds, int hundredths)
        {
            Timer = new Timemachine(minutes, seconds, hundredths);
        }

        public override string ToString()
        {
            return Timer.ToString();
        }
    }
}
