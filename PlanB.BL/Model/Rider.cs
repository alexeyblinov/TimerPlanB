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

        private int riderId;
        private string name;
        private string surname;
        private Gender gender;
        private string location;
        private string team;
        private int tryFirst;
        private int trySecond;
        private int bestResult;
        private int rank;
        private string previousClassId;
        private string resultClassId;

        /// <summary>
        /// Стартовый номер участника.
        /// </summary>
        public int RiderId { 
            get
            {
                return riderId;
            }
            set
            {
                if(value <= 0)
                {
                    throw new ArgumentException("Стартовый номер должен быть натуральным числом.", nameof(riderId));
                }
                else
                {
                    riderId = value;
                }
            }
        }
        /// <summary>
        /// Имя участника.
        /// </summary>
        public string Name {
            get
            {
                return name;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Имя не указано.", nameof(name));
                }
                else
                {
                    name = value;
                }
            }
        }
        /// <summary>
        /// Фамилия участника.
        /// </summary>
        public string Surname {
            get
            {
                return surname;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Фамилия не указана.", nameof(surname));
                }
                else
                {
                    surname = value;
                }
            }
        }
        /// <summary>
        /// Половая принадлежность. Пока реализована толька традиционная.
        /// </summary>
        public Gender Gender {
            get 
            {
                return gender;
            }
            set 
            { 
                if(value == null)
                {
                    throw new ArgumentException("Пол не определён.", nameof(gender));
                }
                else
                {
                    gender = value;
                }
            } 
        }
        /// <summary>
        /// Город, который представляет участник.
        /// </summary>
        public string Location {
            get
            {
                return location;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Населённый пункт не указан.", nameof(location));
                }
                else
                {
                    location = value;
                }
            }
        }
        /// <summary>
        /// Команда, за которую выступает участник.
        /// </summary>
        public string Team {
            get
            {
                return team;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Команда не указана.", nameof(team));
                }
                else
                {
                    team = value;
                }
            }
        }
        /// <summary>
        /// Является ли транспортное средство круизёром.
        /// </summary>
        public bool IsCruiser { get; set; }

        /// <summary>
        /// Результат первого заезда.
        /// </summary>
        public int TryFirst {
            get
            {
                return tryFirst;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Значение времени 1 попытки не может быть отрицательным.", nameof(tryFirst));
                }
                else
                {
                    tryFirst = value;
                }
            }
        }
        /// <summary>
        /// Результат второго заезда.
        /// </summary>
        public int TrySecond
        {
            get
            {
                return trySecond;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Значение времени 2 попытки не может быть отрицательным.", nameof(trySecond));
                }
                else
                {
                    trySecond = value;
                }
            }
        }
        /// <summary>
        /// Лучшее время из двух попыток.
        /// </summary>
        public int BestResult
        {
            get
            {
                return bestResult;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Значение времени 1 попытки не может быть отрицательным.", nameof(bestResult));
                }
                else
                {
                    bestResult = value;
                }
            }
        }
        /// <summary>
        /// Место участника в классе.
        /// </summary>
        public int Rank
        {
            get
            {
                return rank;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Номер позиции должен быть натуральным числом.", nameof(rank));
                }
                else
                {
                    rank = value;
                }
            }
        }
        /// <summary>
        /// Класс участника перед соревнованиями.
        /// </summary>
        public string PreviousClassId
        {
            get
            {
                return previousClassId;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value) || !Enum.IsDefined(typeof(ClassName), value))
                {
                    throw new ArgumentException("Идентификатор класса не определён.", nameof(previousClassId));
                }
                else
                {
                    previousClassId = value;
                }
            }
        }
        /// <summary>
        /// Класс участника по итогам соревнования.
        /// </summary>
        public string ResultClassId
        {
            get
            {
                return resultClassId;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value) || !Enum.IsDefined(typeof(ClassName), value))
                {
                    throw new ArgumentException("Идентификатор класса не определён.", nameof(resultClassId));
                }
                else
                {
                    resultClassId = value;
                }
            }
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
            string result = string.Concat("#", RiderId, " ", Name, " ", Surname, " [", PreviousClassId, "]");
            return result;
        }

    }

}
