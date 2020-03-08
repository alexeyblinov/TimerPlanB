using System;

namespace PlanB.BL.Model
{
    [Serializable]
    public class RiderNovice : Rider
    {
        public string ClassId { get; }
        public int TryFirst { get; set; }
        public int TrySecond { get; set; }
        public int Rank { get; set; }
        /// <summary>
        /// Класс участника, присвоенный по результатам соревнования.
        /// </summary>
        public string ResultClassId { get; set; }

        public RiderNovice(int riderId, string classId) : base(riderId)
        {
            ClassId = classId;
        }

        public RiderNovice(int riderId, 
                           string classId, 
                           string name,
                           string surname,
                           Gender gender,
                           string location,
                           string team) : base(riderId, name, surname, gender, location, team)
        {
            if (string.IsNullOrWhiteSpace(classId))
            {
                throw new ArgumentException("Class is not exist.", nameof(classId));
            }

            TryFirst = 0;
            TrySecond = 0;
            Rank = 0;
        }

        public override string ToString()
        {
            string result = string.Concat(RiderId.ToString(), " ", Name);
            return result;
        }


    }
}
