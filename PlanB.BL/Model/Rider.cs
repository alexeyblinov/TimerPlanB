using System;

namespace PlanB.BL.Model
{
    [Serializable]
    public class Rider
    {
        /// <summary>
        /// Rider's start number. Sets when creating an object.
        /// </summary>
        public int RiderId { get; }
        /// <summary>
        /// Rider's name. Sets when creating an object.
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Rider's surname. Sets when creating an object.
        /// </summary>
        public string Surname { get; }
        /// <summary>
        /// Rider's gender. Sets when creating an object.
        /// </summary>
        public Gender Gender { get; }
        /// <summary>
        /// Rider's hometown. Sets when creating an object.
        /// </summary>
        public string Location { get; }
        /// <summary>
        /// Rider's team. Sets when creating an object.
        /// </summary>
        public string Team { get; }

        /// <summary>
        /// Create a new rider.
        /// </summary>
        /// <param name="riderId"> Rider's start number. </param>
        /// <param name="name"> Rider's name. </param>
        /// <param name="surname"> Rider's surname. </param>
        /// <param name="gender"> Rider's gender. </param>
        /// <param name="location"> Rider's hometown. </param>
        /// <param name="team"> Rider's team. </param>
        public Rider(int riderId, 
                     string name, 
                     string surname, 
                     Gender gender, 
                     string location, 
                     string team)
        {
            if (riderId <= 0)
            {
                throw new ArgumentOutOfRangeException("ID must be from 1 to 99.", nameof(riderId));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name is not exist.", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(surname))
            {
                throw new ArgumentException("Surname is not exist.", nameof(surname));
            }

            if (gender is null)
            {
                throw new ArgumentNullException("You should use M or F.", nameof(gender));
            }

            if (string.IsNullOrWhiteSpace(location))
            {
                throw new ArgumentException("Hometown is not exist.", nameof(location));
            }

            if (string.IsNullOrWhiteSpace(team))
            {
                throw new ArgumentException("Team is not exist.", nameof(team));
            }

            RiderId = riderId;
            Name = name;
            Surname = surname;
            Gender = gender;
            Location = location;
            Team = team;
        }

        public override string ToString()
        {
            string result = string.Concat(RiderId.ToString(), " ", Name);  
            return result;
        }

    }

}
