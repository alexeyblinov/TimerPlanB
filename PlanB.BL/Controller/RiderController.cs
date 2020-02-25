using PlanB.BL.Model;
using System;
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
        public RiderController(int startNumber, string name, string surname, string riderGender, string location, string team)
        {
            Gender gender = new Gender(riderGender);

            Rider = new Rider(startNumber, name, surname, gender, location, team);
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
