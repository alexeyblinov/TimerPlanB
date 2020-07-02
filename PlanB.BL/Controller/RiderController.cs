using PlanB.BL.Model;
using PlanB.Validators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace PlanB.BL.Controller
{
    [Serializable]
    /// <summary>
    /// Контроллер участника.
    /// </summary>
    public class RiderController
        
    {
        public RiderValidator riderValidator;

        /// <summary>
        /// Список участников.
        /// </summary>
        public List<Rider> Riders { get; private set; }

        /// <summary>
        /// Текущий участник.
        /// </summary>
        public Rider CurrentRider { get; }

        /// <summary>
        /// Конструктор без параметров для контроллера участника.
        /// </summary>
        public RiderController() { }

        /// <summary>
        /// Создать нового участника, если его нет в списке, иначе вернуть данные из списка.
        /// </summary>
        /// <param name="startNumber"> Стартовый номер участника (Rider.riderId) </param>
        /// <param name="classId"> Класс участника </param>
        public RiderController(int startNumber, string classId)
        {
            riderValidator = new RiderValidator();

            Riders = GetRiders();

            CurrentRider = Riders.SingleOrDefault(r => r.RiderId == startNumber);

            if(CurrentRider == null)
            {
                CurrentRider = new Rider(startNumber)
                {
                    PreviousClassId = classId,
                    ResultClassId = classId
                };
                var validationResult = riderValidator.Validate(CurrentRider);
                if (!validationResult.IsValid)
                {
                    throw new ArgumentException(validationResult.ToString());
                }
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

        /// <summary>
        /// Загрузка списка участников из файла.
        /// </summary>
        public void Load()
        {
            var formatter = new BinaryFormatter();

            using (var fileStream = new FileStream("riders.dat", FileMode.OpenOrCreate))
            {
                if (fileStream.Length != 0 && formatter.Deserialize(fileStream) is List<Rider> riders)
                {
                    Riders = riders;
                }
                else
                {
                    Riders = new List<Rider>();
                }
            }
        }

        public override string ToString()
        {
            return CurrentRider.ToString();
        }
    }
}
