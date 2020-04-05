using System;

namespace PlanB.BL.Model
{
    [Serializable]
    public class Rider : IComparable
    {
        /// <summary>
        /// Максимально возможное время заезда.
        /// </summary>
        public const int MAXTIME = 359999;
        /// <summary>
        /// Стартовый номер участника.
        /// </summary>
        public int RiderId 
        { get; set;
            //get 
            //{ 
            //    if(RiderId <= 0)
            //    {
            //        throw new ArgumentOutOfRangeException("Something wrong with Rider ID.", nameof(RiderId));
            //    }
            //    else
            //    {
            //        return RiderId;
            //    }
            //}
            //private set
            //{
            //    if (value < 0)
            //    {
            //        throw new ArgumentOutOfRangeException("Rider ID cannot be negative.", nameof(RiderId));
            //    }
            //    else
            //    {
            //        RiderId = value;
            //    }
            //}
        }
        /// <summary>
        /// Имя участника.
        /// </summary>
        public string Name 
        {
            get; set;
            //get => Name;
            //set 
            //{
            //    if (string.IsNullOrWhiteSpace(value)) {
            //        throw new ArgumentException("Name cannot be null or whitespace.", nameof(Name));
            //    }
            //    Name = value;
            //}
        }
        /// <summary>
        /// Фамилия участника.
        /// </summary>
        public string Surname 
        {
            get; set;
            //get => Surname;
            //set
            //{
            //    if (string.IsNullOrWhiteSpace(value))
            //    {
            //        throw new ArgumentException("Surname cannot be null or whitespace.", nameof(Surname));
            //    }
            //    Surname = value;
            //}
        }
        /// <summary>
        /// Половая принадлежность. Пока реализована толька традиционная.
        /// </summary>
        public Gender Gender { get; set; }
        /// <summary>
        /// Город, который представляет участник.
        /// </summary>
        public string Location 
        {
            get; set;
            //get => Location;
            //set 
            //{
            //    if (string.IsNullOrWhiteSpace(value))
            //    {
            //        throw new ArgumentException("Location cannot be null or whitespace.", nameof(Location));
            //    }
            //    Location = value;
            //} 
        }
        /// <summary>
        /// Команда, за которую выступает участник.
        /// </summary>
        public string Team
        {
            get; set;
            //get => Team;
            //set
            //{
            //    if (string.IsNullOrWhiteSpace(value))
            //    {
            //        throw new ArgumentException("Team cannot be null or whitespace.", nameof(Team));
            //    }
            //    Team = value;
            //}
        }
        /// <summary>
        /// Является ли транспортное средство круизёром.
        /// </summary>
        public bool IsCruiser { get; set; }
        /// <summary>
        /// Результат первого заезда.
        /// </summary>
        public int TryFirst 
        {
            get; set;
            //get => TryFirst;
            //set 
            //{
            //    if (value < 0) 
            //    {
            //        throw new ArgumentOutOfRangeException("First try cannot be negative.", nameof(TryFirst));
            //    }
            //    TryFirst = value;
            //} 
        }
        /// <summary>
        /// Результат второго заезда.
        /// </summary>
        public int TrySecond
        {
            get; set;
            //get => TrySecond;
            //set
            //{
            //    if (value < 0)
            //    {
            //        throw new ArgumentOutOfRangeException("Second try cannot be negative.", nameof(TrySecond));
            //    }
            //    TrySecond = value;
            //}
        }
        /// <summary>
        /// Лучшее время из двух попыток.
        /// </summary>
        public int BestResult
        {
            get; set;
            //get => BestResult;
            //set
            //{
            //    if (value < 0)
            //    {
            //        throw new ArgumentOutOfRangeException("Best result cannot be negative.", nameof(BestResult));
            //    }
            //    BestResult = value;
            //}
        }
        /// <summary>
        /// Место участника в классе.
        /// </summary>
        public int Rank
        {
            get; set;
            //get => Rank;
            //set
            //{
            //    if (value < 0)
            //    {
            //        throw new ArgumentOutOfRangeException("Rank cannot be negative.", nameof(Rank));
            //    }
            //    Rank = value;
            //}
        }
        /// <summary>
        /// Класс участника перед соревнованиями.
        /// </summary>
        public string PreviousClassId
        {
            get; set;
            //get => PreviousClassId;
            //set
            //{
            //    if (string.IsNullOrWhiteSpace(value))
            //    {
            //        throw new ArgumentException("Previous class ID cannot be null or whitespace.", nameof(PreviousClassId));
            //    }
            //    PreviousClassId = value;
            //}
        }
        /// <summary>
        /// Класс участника по итогам соревнования.
        /// </summary>
        public string ResultClassId
        {
            get; set;
            //get => ResultClassId;
            //set
            //{
            //    if (string.IsNullOrWhiteSpace(value))
            //    {
            //        throw new ArgumentException("Result class ID cannot be null or whitespace.", nameof(ResultClassId));
            //    }
            //    ResultClassId = value;
            //}
        }

        /// <summary>
        /// Конструктор для создания нового участника.
        /// </summary>
        /// <param name="riderId"> Стартовый номер. </param>
        /// <param name="name"> Имя. </param>
        /// <param name="surname"> Фамилия. </param>
        /// <param name="gender"> Пол. </param>
        /// <param name="location"> Город, который представляет участник. </param>
        /// <param name="team"> Название команды. </param>
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

        /// <summary>
        /// Конструктор для создания или поиска участника по стартовому номеру.
        /// </summary>
        /// <param name="riderId"></param>
        public Rider(int riderId)
        {
            if (riderId <= 0)
            {
                throw new ArgumentOutOfRangeException("ID cannot be negative or 0 (Recommended option 1-99).", nameof(riderId));
            }
            RiderId = riderId;
        }

        // Сравнение участников по лучшему результату.
        public int CompareTo(object obj)
        {
            if (obj is null)
            {
                return 1;
            }

            Rider rider = obj as Rider;
            if(rider == null)
            {
                throw new ArgumentException("The comparable object is not a Rider.", nameof(rider));
            }
            return this.BestResult.CompareTo(rider.BestResult);
        }

        public override string ToString()
        {
            string result = string.Concat(Rank, ":  #", RiderId, " ", Surname, " 1: ", TryFirst, " 2: ", TrySecond, " Total: ", BestResult, " Class: ", PreviousClassId, " Resalt: ", ResultClassId);
            return result;
        }

    }

}
