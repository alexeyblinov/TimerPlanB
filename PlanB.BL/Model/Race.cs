using PlanB.BL.Controller;
using System;
using System.Collections.Generic;

namespace PlanB.BL.Model
{
    /// <summary>
    /// Модель обработчика заезда. Принимает райдера, его время и штраф, устанавливает и возвращает позицию в классе.
    /// </summary>
    public class Race
    {
        private RiderController CurrentRider;
        private List<Rider> Riders;
        public int LapTime { get; }
        public int Penalty { get; }

        /// <summary>
        /// Создает экземпляр заезда. Принимает райдера, его время и штраф.
        /// </summary>
        /// <param name="currentRider"> Текущий участник </param>
        /// <param name="lapTime"> Время круга текущего усастника </param>
        /// <param name="penalty"> Штрафное время текущего участника </param>
        public Race(RiderController currentRider, int lapTime, int penalty)
        {
            if (lapTime <= 0)
            {
                throw new ArgumentOutOfRangeException("ID must be from 1 to 99.", nameof(lapTime));
            }

            if (penalty <= 0)
            {
                throw new ArgumentOutOfRangeException("ID must be from 1 to 99.", nameof(penalty));
            }

            CurrentRider = currentRider ?? throw new ArgumentNullException(nameof(currentRider));
            LapTime = lapTime;
            Penalty = penalty;
            Riders = CurrentRider.GetRiders();
        }

        /// <summary>
        /// Устанавливает и возвращает позицию текущего участника.
        /// </summary>
        public void ChangeRank()
        {
            // TODO Здесь должна быть проверка, есть ли свободный слот для записи времени.
            // Записать время в первый, если он свободен, иначе во второй.
            // Если результат записан в первый слот, вставить райдера на позицию согласно результата.
            // Если результат записан во второй слот, сравнить с первым слотом.
            // Если в первом слоте время меньше - ничего не делать.
            // Если во втором слоте время больше, изменить положение райдера согласно новому результату.
            // И всё это перенести в контроллер.
        }
    }
}
