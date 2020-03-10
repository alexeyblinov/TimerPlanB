using System;

namespace PlanB.BL.Model
{
    [Serializable]
    /// <summary>
    /// Описание класса Новичок (N). 
    /// </summary>
    public class RiderNovice : Rider
    {
        /// <summary>
        /// Идентификатор класса участника B, C, D, или N.
        /// </summary>
        public string ClassId { get; }
        /// <summary>
        /// Время в сотых долях сукунды первого заезда.
        /// </summary>
        public int TryFirst { get; set; }
        /// <summary>
        /// Время в сотых долях секунды второго заезда.
        /// </summary>
        public int TrySecond { get; set; }
        /// <summary>
        /// Место в классе по итогам заездов.
        /// </summary>        
        public int Rank { get; set; }
        /// <summary>
        /// Класс участника, присвоенный по результатам соревнования.
        /// </summary>
        public string ResultClassId { get; set; }

        /// <summary>
        /// Конструктор для создания экземпляра участника класса Новичок по стартовому номеру.
        /// </summary>
        /// <param name="riderId"> Стартовый номер. </param>
        /// <param name="classId"> Идентификатор класса. </param>
        public RiderNovice(int riderId, string classId) : base(riderId)
        {
            ClassId = classId;
        }

        /// <summary>
        /// Конструктор для создания экземпляра участника класса Новичок.
        /// </summary>
        /// <param name="riderId"> Стартовый номер. </param>
        /// <param name="classId"> Идентификатор класса. </param>
        /// <param name="name"> Имя. </param>
        /// <param name="surname"> Фамилия. </param>
        /// <param name="gender"> Пол. </param>
        /// <param name="location"> Город участника. </param>
        /// <param name="team"> Название команды. </param>
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
