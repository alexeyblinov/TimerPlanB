using PlanB.BL.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        /// Some current rider.
        /// </summary>
        public Rider CurrentRider { get; }

        /// <summary>
        /// Create a new rider or get from a saved list.
        /// </summary>
        /// <param name="startNumber"> Rider's start number (Rider.riderId) </param>
        public RiderController(int startNumber)
        {
            if (startNumber <= 0)
            {
                throw new ArgumentOutOfRangeException("Start number must be from 1 to 99.", nameof(startNumber));
            }
            Riders = GetRiders();

            CurrentRider = Riders.SingleOrDefault(r => r.RiderId == startNumber);

            if(CurrentRider == null)
            {
                CurrentRider = new Rider(startNumber);
                Riders.Add(CurrentRider);
                Save();
            }
        }


        /// <summary>
        /// Load Rider's data from file or create a new list.
        /// </summary>
        /// <returns></returns>
        private List<Rider> GetRiders()
        {
            var formatter = new BinaryFormatter();

            using (var fileStream = new FileStream("riders.dat", FileMode.OpenOrCreate))
            {
                if (fileStream.Length != 0 && formatter.Deserialize(fileStream) is List<Rider> riders)
                {
                    return riders;
                }
                else
                {
                    return new List<Rider>();
                }
            }
        }

        public void SetNewRiderData(string name, 
                                    string surname, 
                                    string gender, 
                                    string location, 
                                    string team)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name is not exist.", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(surname))
            {
                throw new ArgumentException("Surname is not exist.", nameof(surname));
            }

            if (string.IsNullOrWhiteSpace(gender))
            {
                throw new ArgumentException("You should use M or F.", nameof(gender));
            }

            if (string.IsNullOrWhiteSpace(location))
            {
                throw new ArgumentException("Hometown is not exist.", nameof(location));
            }

            if (string.IsNullOrWhiteSpace(team))
            {
                throw new ArgumentException("Team is not exist.", nameof(team));
            }

            CurrentRider.Name = name;
            CurrentRider.Surname = surname;
            CurrentRider.Gender = new Gender(gender);
            CurrentRider.Location = location;
            CurrentRider.Team = team;
            Save();
        }

        /// <summary>
        /// Save the list of riders to a file.
        /// </summary>
        public void Save()
        {
            var formatter = new BinaryFormatter();

            using(var fileStream = new FileStream("riders.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fileStream, Riders);
            } 
        }
    }
}
