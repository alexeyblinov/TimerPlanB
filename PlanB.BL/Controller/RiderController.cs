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
        /// Список участников.
        /// </summary>
        public List<Rider> Riders { get; }

        /// <summary>
        /// Текущий участник.
        /// </summary>
        public Rider CurrentRider { get; }

        /// <summary>
        /// Конструктор без параметров для контроллера участника (c).
        /// </summary>
        public RiderController() { }

        /// <summary>
        /// Создать нового участника, если его нет в списке, иначе вернуть данные из списка.
        /// </summary>
        /// <param name="startNumber"> Стартовый номер участника (Rider.riderId) </param>
        public RiderController(int startNumber, string classId)
        {
            // Строка списка возможных классов участников.
            
            if (startNumber <= 0)
            {
                throw new ArgumentOutOfRangeException("Start number must be from 1 to 99.", nameof(startNumber));
            }

            Riders = GetRiders();

            CurrentRider = Riders.SingleOrDefault(r => r.RiderId == startNumber);

            if(CurrentRider == null)
            {
                CurrentRider = new Rider(startNumber)
                {
                    PreviousClassId = classId,
                    ResultClassId = classId
                };
                Riders.Add(CurrentRider);
                Save();
            }
        }


        /// <summary>
        /// Загрузка списка участников из файла, если файл пустой или отсутствует, создание нового списка.
        /// </summary>
        /// <returns> Список всех участников. </returns>
        internal List<Rider> GetRiders()
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

        /// <summary>
        /// Добавление данных об участнике, если у него есть только номер.
        /// </summary>
        /// <param name="name"> Имя. </param>
        /// <param name="surname"> Фамилия. </param>
        /// <param name="gender"> Пол. </param>
        /// <param name="location"> Город, который представляет участник. </param>
        /// <param name="team"> Название команды. </param>
        /// /// <param name="isCruiser"> Является ли ТС круизёром. </param>
        public void SetNewRiderData(string name, 
                                    string surname, 
                                    string gender, 
                                    string location, 
                                    string team,
                                    bool isCruiser = false)
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
            CurrentRider.TryFirst = Rider.MAXTIME;
            CurrentRider.TrySecond = Rider.MAXTIME;
            CurrentRider.BestResult = 0;
            CurrentRider.Rank = 0;
            CurrentRider.IsCruiser = isCruiser;
            Save();
        }

        /// <summary>
        /// Сохранить список участников в файл.
        /// </summary>
        public void Save()
        {
            var formatter = new BinaryFormatter();

            using(var fileStream = new FileStream("riders.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fileStream, Riders);
            } 
        }

        public override string ToString()
        {
            return CurrentRider.ToString();
        }
    }
}
