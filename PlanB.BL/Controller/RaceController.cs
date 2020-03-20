using PlanB.BL.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlanB.BL.Controller
{
    public static class RaceController
    {
        /// <summary>
        /// Устанавливает результат заезда и корректирует позицию участника в классе.
        /// </summary>
        /// <param name="riderController"> Контроллер участника. </param>
        /// <param name="lapTime"> Время круга в сотых. </param>
        /// <param name="penalty"> Штрафные баллы в сотых долях секунды. </param>
        /// <param name="newTryFirst"> При значении 1 перезаписывает результат первой попытки. </param>
        /// <param name="newTrySecond"> При значении 1 перезаписывает результат второй попытки. </param>
        /// <param name="infinity"> Если значение 1 перезаписывать новый результат вместо худшего, иначе после заполнения двух попыток не записывать больше. </param>
        public static void ChangeRank(RiderController riderController,
                               Rider rider,
                               int lapTime,
                               int penalty,
                               bool newTryFirst = false,
                               bool newTrySecond = false,
                               bool infinity = false)
        {
            if (riderController is null)
            {
                throw new ArgumentNullException("Rider Controller cannot be null.", nameof(riderController));
            }

            if (rider is null)
            {
                throw new ArgumentNullException("Rider cannot be null.", nameof(rider));
            }
            if (lapTime < 0)
            {
                throw new ArgumentOutOfRangeException("Lap Time must be positive.", nameof(lapTime));
            }

            if (penalty < 0)
            {
                throw new ArgumentOutOfRangeException("Penalty must be positive.", nameof(penalty));
            }
            if (newTryFirst == true && newTrySecond == true)
            {
                throw new ArgumentException("Only one parameter (newTryFirst/newTrySecond) can be 'true'.");
            }

            var total = lapTime + penalty;
            if (total > Rider.MAXTIME)
            {
                throw new ArgumentOutOfRangeException("Lap Time + Penalty mast not exceed 59:59:99.", nameof(total));
            }


            if(newTryFirst == true || newTrySecond == true)
            {
                ReTry(rider, total, newTryFirst);
            }
            else
            {
                if (infinity == true)
                {
                    IfInfinity(rider, total);
                }
                else
                {
                    SetResults(rider, total);
                }
            }

            SetBestResult(rider);

            riderController.Riders.Sort();
            var i = 1;
            foreach (var r in riderController.Riders)
            {
                r.Rank = i;
                i++;
            }
            riderController.Save();
        }

        /// <summary>
        /// Установить (записать в свойство текущего экземпляра Raider) лучшее время сравнением результатов двух попыток.
        /// </summary>
        /// <param name="rider"> Текущий участник. </param>
        private static void SetBestResult(Rider rider)
        {
            if (rider.TryFirst <= rider.TrySecond
                && rider.TryFirst > 0)
            {
                rider.BestResult = rider.TryFirst;
            }
            else
            {
                rider.BestResult = rider.TrySecond;
            }
        }

        /// <summary>
        /// Перезаезд. Перезапись первого или второго результата в зависимотти от значений newTryFirst и newTrySecond.
        /// </summary>
        /// <param name="rider"> Текущий участник. </param>
        /// <param name="total"> Итоговое время заезда. </param>
        /// <param name="newTryFirst"> Если истинно, переписать первую попытку, иначе вторую. </param>
        private static void ReTry(Rider rider, int total, bool newTryFirst)
        {
            if(newTryFirst == true)
            {
                rider.TryFirst = total;
            }
            else
            {
                rider.TrySecond = total;
            }
        }

        /// <summary>
        /// Запись временного результата заезда в свободный слот. Если оба заняты, то никуда.
        /// </summary>
        /// <param name="rider"> Текущий участник. </param>
        /// <param name="total"> Итоговое время заезда. </param>
        private static void SetResults(Rider rider, int total)
        {
            if (rider.TryFirst == Rider.MAXTIME)
            {
                rider.TryFirst = total;
            }
            else if (rider.TrySecond == Rider.MAXTIME)
            {
                rider.TrySecond = total;
            }
        }

        /// <summary>
        /// Если установлен флаг бесконечной перезаписи лучшего нового результата, перезаписывает его.
        /// </summary>
        /// <param name="rider"> Текущий участник. </param>
        /// <param name="total"> Итоговое время заезда. </param>
        private static void IfInfinity(Rider rider, int total)
        {
            if (rider.TryFirst <= rider.TrySecond && rider.TryFirst > total && total > 0)
            {
                rider.TryFirst = total;
            }
            else if(rider.TrySecond > total && total > 0)
            {
                rider.TrySecond = total;
            }
        }

        /// <summary>
        /// Установка классов участников, полученных по результатам соревнований.
        /// Если класс соревнования совпадает с классом лучшего участника, рассчитать новые классы. 
        /// Если класс соревнования ниже, найти лучшего участника в класе соревнования и использовать 
        /// его время для рассчёта новых классов.
        /// </summary>
        /// <param name="riderController"> Контроллер участника текущего класса. </param>
        /// <param name="competitionClass"> Класс соревнования. Должен задаваться после регистрации всех участников. </param>
        public static void SetResultClassId(RiderController riderController, string competitionClass)
        {
            if (riderController is null)
            {
                throw new ArgumentNullException("Rider Controller cannot be null.", nameof(riderController));
            }

            if (string.IsNullOrWhiteSpace(competitionClass))
            {
                throw new ArgumentNullException("Competition class cannot be null or whitespace.", nameof(competitionClass));
            }

            var bestTime = riderController.Riders.First().BestResult;
            var bestClass = riderController.Riders.First().ResultClassId;
            Dictionary<string, decimal> coefficient = new Dictionary<string, decimal>(9);
            coefficient.Add("B", 1m);
            coefficient.Add("C1", 1.05m);
            coefficient.Add("C2", 1.1m);
            coefficient.Add("C3", 1.15m);
            coefficient.Add("D1", 1.2m);
            coefficient.Add("D2", 1.3m);
            coefficient.Add("D3", 1.4m);
            coefficient.Add("D4", 1.5m);
            coefficient.Add("N", 1.6m);

            if(bestClass == competitionClass)
            {

            }
        }
    }
}
