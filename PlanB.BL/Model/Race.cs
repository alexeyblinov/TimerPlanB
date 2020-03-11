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
        private RiderController CurrentRiderController;
        private List<Rider> Riders;
        public int LapTime { get; }
        public int Penalty { get; }


        public Race() { }
        /// <summary>
        /// Создает экземпляр заезда. Принимает райдера, его время и штраф.
        /// </summary>
        /// <param name="currentRiderController"> Текущий участник </param>
        /// <param name="lapTime"> Время круга текущего усастника </param>
        /// <param name="penalty"> Штрафное время текущего участника </param>
        
        //public Race(RiderController currentRiderController, int lapTime, int penalty)
        //{
        //    if (lapTime <= 0)
        //    {
        //        throw new ArgumentOutOfRangeException("ID must be from 1 to 59.", nameof(lapTime));
        //    }

        //    if (penalty <= 0)
        //    {
        //        throw new ArgumentOutOfRangeException("ID must be from 1 to 59.", nameof(penalty));
        //    }

        //    CurrentRiderController = currentRiderController ?? throw new ArgumentNullException(nameof(currentRiderController));
        //    LapTime = lapTime;
        //    Penalty = penalty;
        //    Riders = CurrentRiderController.GetRiders();
        //}

        /// <summary>
        /// Устанавливает время круга и возвращает позицию текущего участника.
        /// </summary>
        public static void ChangeRank(RiderController riderController, int lapTime, int penalty)
        {
            if (riderController is null)
            {
                throw new ArgumentNullException("RiderController cannot be null.", nameof(riderController));
            }
            if (lapTime <= 0 || lapTime > 59)
            {
                throw new ArgumentOutOfRangeException("ID must be from 1 to 59.", nameof(lapTime));
            }

            if (penalty <= 0 || penalty > 59)
            {
                throw new ArgumentOutOfRangeException("ID must be from 1 to 59.", nameof(penalty));
            }

            var total = lapTime + penalty;
            if(riderController.CurrentRider.TryFirst == 0)
            {
                riderController.CurrentRider.TryFirst = total;
            }
            else if(riderController.CurrentRider.TryFirst >= riderController.CurrentRider.TrySecond)
            {
                riderController.CurrentRider.TryFirst = total;
            }
            else
            {
                riderController.CurrentRider.TrySecond = total;
            }

            if(riderController.CurrentRider.TryFirst <= riderController.CurrentRider.TrySecond 
               && riderController.CurrentRider.TryFirst > 0)
            {
                riderController.CurrentRider.BestResult = riderController.CurrentRider.TryFirst;
            } 
            else
            {
                riderController.CurrentRider.BestResult = riderController.CurrentRider.TrySecond;
            }

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
