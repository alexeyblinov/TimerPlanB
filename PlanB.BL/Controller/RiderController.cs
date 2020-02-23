using PlanB.BL.Model;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace PlanB.BL.Controller
{
    /// <summary>
    /// Rider controller.
    /// </summary>
    public class RiderController
        
    {
        /// <summary>
        /// umm just a rider field...
        /// </summary>
        public Rider Rider { get; }

        /// <summary>
        /// Constructor. Load Rider's data from file.
        /// </summary>
        /// <param name="rider"></param>
        /// <returns> Rider </returns>
        public RiderController()
        {
            var formatter = new BinaryFormatter();

            using (var fileStream = new FileStream("riders.dat", FileMode.OpenOrCreate))
            {
                if (formatter.Deserialize(fileStream) is Rider rider)
                {
                    Rider = rider;
                }
                else
                {
                    // TODO: to do thomething if exception is here..
                }
            }
        }

        /// <summary>
        /// Creat new rider controller.
        /// </summary>
        /// <param name="raider"> Rider </param>
        public RiderController(Rider rider)
        {
            if (rider is null)
            {
                throw new ArgumentNullException("Rider can't be null", nameof(rider));
            }

            Rider = rider;
        }

        /// <summary>
        /// Save Rider/s data to file.
        /// </summary>
        /// <param name="rider"></param>
        /// <returns></returns>
        public void Save(Rider rider)
        {
            var formatter = new BinaryFormatter();

            using(var fileStream = new FileStream("riders.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fileStream, Rider);
            } 
        }
    }
}
