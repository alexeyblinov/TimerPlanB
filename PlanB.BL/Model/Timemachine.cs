using System;

namespace PlanB.BL.Model
{
    /// <summary>
    /// Timer. Accepts mm:ss:00, returns handredths.
    /// </summary>
    public class Timemachine
    {
        public int Minutes { get; }
        public int Seconds { get; }
        public int Hundredths { get; }

        /// <summary>
        /// Create a new timer.
        /// </summary>
        public Timemachine() : this (0, 0, 0) 
        {
        }

        /// <summary>
        /// Create a new timer.
        /// </summary>
        /// <param name="minutes"> minutes </param>
        /// <param name="seconds"> seconds </param>
        /// <param name="hundredths"> hundredths </param>
        public Timemachine(int minutes, int seconds, int hundredths)
        {
            if (minutes < 0 || minutes > 99)
            {
                throw new ArgumentOutOfRangeException("Minutes must be from 0 to 99.", nameof(minutes));
            }
            if (seconds < 0 || seconds > 59)
            {
                throw new ArgumentOutOfRangeException("Seconds must be from 0 to 59.", nameof(seconds));
            }
            if (hundredths < 0 || hundredths > 99)
            {
                throw new ArgumentOutOfRangeException("Hundredths must be from 0 to 99.", nameof(hundredths));
            }
            Minutes = minutes;
            Seconds = seconds;
            Hundredths = hundredths;
        }

        public override string ToString()
        {
            var result = string.Concat(Minutes.ToString(), ":", Seconds.ToString(), ":", Hundredths.ToString());
            return result;
        }
    }
}
