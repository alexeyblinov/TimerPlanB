using PlanB.BL.Model;
using System;

namespace PlanB.BL.Controller
{
    public class RaceController
    {
        /// <summary>
        /// Максимально возможное время заезда.
        /// </summary>
        const int MAXTIME = 359999;

        /// <summary>
        /// Устанавливает результат заезда и корректирует позицию участника в классе.
        /// </summary>
        /// <param name="riderController"> Контроллер участника. </param>
        /// <param name="lapTime"> Время круга в сотых. </param>
        /// <param name="penalty"> Штрафные баллы в сотых долях секунды. </param>
        /// <param name="newTryFirst"> При значении 1 перезаписывает результат первой попытки. </param>
        /// <param name="newTrySecond"> При значении 1 перезаписывает результат второй попытки. </param>
        /// <param name="infinity"> Если значение 1 перезаписывать новый результат вместо худшего, иначе после заполнения двух попыток не записывать больше. </param>
        public void ChangeRank(RiderController riderController,
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
            if (total > MAXTIME)
            {
                throw new Exception("Lap Time + Penalty mast not exceed 59:59:99.");
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
        private void SetBestResult(Rider rider)
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
        private void ReTry(Rider rider, int total, bool newTryFirst)
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
        private void SetResults(Rider rider, int total)
        {
            if (rider.TryFirst == MAXTIME)
            {
                rider.TryFirst = total;
            }
            else if (rider.TrySecond == MAXTIME)
            {
                rider.TrySecond = total;
            }
        }

        /// <summary>
        /// Если установлен флаг бесконечной перезаписи лучшего нового результата, перезаписывает его.
        /// </summary>
        /// <param name="rider"> Текущий участник. </param>
        /// <param name="total"> Итоговое время заезда. </param>
        private void IfInfinity(Rider rider, int total)
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
    }
}
