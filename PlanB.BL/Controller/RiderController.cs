using PlanB.BL.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace PlanB.BL.Controller
{
    [Serializable]
    /// <summary>
    /// Rider controller.
    /// </summary>
    public class RiderController
        
    {
        /// <summary>
        /// Riders list.
        /// </summary>
        public List<Rider> Riders { get; }

        /// <summary>
        /// Constructor. Load Rider's data from file.
        /// </summary>
        /// <param name="rider"></param>
        /// <returns> Rider </returns>
        private List<Rider> GetRiders()
        {
            var formatter = new BinaryFormatter();

            using (var fileStream = new FileStream("riders.dat", FileMode.OpenOrCreate))
            {
                if (formatter.Deserialize(fileStream) is List<Rider> riders)
                {
                    return riders;
                }
                else
                {
                    return new List<Rider>();
                }
            }
        }

        /// <summary>
        /// Creat new rider controller.
        /// </summary>
        /// <param name="raider"> Rider </param>
        public RiderController(int startNumber)
        {
            if (string.IsNullOrWhiteSpace(startNumber))
            {
                throw new ArgumentNullException("Start number can't be null.", nameof(startNumber));
            }
            Riders = GetRiders();
            
           
        }

        /// <summary>
        /// Save Rider/s data to file.
        /// </summary>
        /// <param name="rider"></param>
        /// <returns></returns>
        public void Save(RiderController riderController)
        {
            var formatter = new BinaryFormatter();

            using(var fileStream = new FileStream("riders.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fileStream, riderController);
            } 
        }
    }
}
