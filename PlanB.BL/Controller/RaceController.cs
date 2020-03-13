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
        /// Есть два поля для двух попыток, в которые изначально записан 359999.
        /// Если значение в первом поле 359999, записать новое значение в первое поле, 
        /// Если значение в первом поле отлично от 359999, то проверить заполнено ли второе поле.
        /// Если второе поле заполнено, значит произошёл перезаезд и нужно заменить худший результат 
        /// из двух полей на только что полученный. Если первое поле отлично от 359999, а второе 359999, 
        /// записать во второе поле.
        /// На основании записанного результата изменить место участника в классе.
        /// </summary>
        /// <param name="riderController"> Контроллер участника. </param>
        /// <param name="lapTime"> Время круга в сотых. </param>
        /// <param name="penalty"> Штрафные баллы в сотых долях секунды. </param>
        /// <param name="infinity"> Если значение 1 перезаписывать новый результат вместо худшего, иначе после заполнения двух попыток не записывать больше. </param>
        public void ChangeRank(RiderController riderController,
                                      Rider rider,
                                      int lapTime,
                                      int penalty,
                                      bool infinity)
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

            var total = lapTime + penalty;
            if (total > MAXTIME)
            {
                throw new Exception("Lap Time + Penalty mast not exceed 59:59:99.");
            }

            riderController.Riders.Remove(rider);
            if (infinity)
            {
                if (rider.TryFirst >= rider.TrySecond)
                {
                    rider.TryFirst = total;
                }
                else
                {
                    rider.TrySecond = total;
                }
            }
            else
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


            if (rider.TryFirst <= rider.TrySecond
               && rider.TryFirst > 0)
            {
                rider.BestResult = rider.TryFirst;
            }
            else
            {
                rider.BestResult = rider.TrySecond;
            }

            riderController.Riders.Add(rider);
            riderController.Riders.Sort();
            var i = 1;
            foreach (var r in riderController.Riders)
            {
                r.Rank = i;
                i++;
            }
            riderController.Save();
        }
    }
}
