﻿using System;

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
        public int RiderId { get; set; }
        /// <summary>
        /// Имя участника.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Фамилия участника.
        /// </summary>
        public string Surname { get; set; }
        /// <summary>
        /// Половая принадлежность. Пока реализована толька традиционная.
        /// </summary>
        public Gender Gender { get; set; }
        /// <summary>
        /// Город, который представляет участник.
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// Команда, за которую выступает участник.
        /// </summary>
        public string Team { get; set; }
        /// <summary>
        /// Является ли транспортное средство круизёром.
        /// </summary>
        public bool IsCruiser { get; set; }
        /// <summary>
        /// Результат первого заезда.
        /// </summary>
        public int TryFirst { get; set; }
        /// <summary>
        /// Результат второго заезда.
        /// </summary>
        public int TrySecond { get; set; }
        /// <summary>
        /// Лучшее время из двух попыток.
        /// </summary>
        public int BestResult { get; set; }
        /// <summary>
        /// Место участника в классе.
        /// </summary>
        public int Rank { get; set; }
        /// <summary>
        /// Класс участника перед соревнованиями.
        /// </summary>
        public string PreviousClassId { get; set; }
        /// <summary>
        /// Класс участника по итогам соревнования.
        /// </summary>
        public string ResultClassId { get; set; }

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
