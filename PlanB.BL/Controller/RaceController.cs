﻿using PlanB.BL.Model;
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
        /// ????????????????????????????????????? TODO ????????????????????????????
        /// </summary>
        /// <param name="riderController"> Контроллер участника текущего класса. </param>
        /// <param name="competitionClass"> Класс соревнования. Должен задаваться после регистрации всех участников. </param>
        
        
        public static string FindCompetitionClassId(RiderController riderController, ref int bestTime)
        {
            if (riderController is null)
            {
                throw new ArgumentNullException("Rider Controller cannot be null.", nameof(riderController));
            }

            // bestClass - класс соревнования.
            var bestClass = string.Empty;
            bestTime = 0;
            bestClass = SetCompetitionClass(riderController, bestClass);
            if (string.IsNullOrEmpty(bestClass))
            {
                throw new ArgumentException("Best class cannot be null.", nameof(bestClass));
            }

            // Находит лучшее время среди участников в классе соревнования.
            bestTime = riderController.Riders.FirstOrDefault(r => r.PreviousClassId == bestClass).BestResult;
            if(bestTime == 0)
            {
                throw new ArgumentException("Best time cannot be 0.", nameof(bestTime));
            }
            return bestClass;
        }

        public static void SetNewClasses(RiderController riderController, string bestClass, int bestTime)
        {
            var coefficients = new Dictionary<string, decimal>(9)
            {
                { "B", 1m },
                { "C1", 1.05m },
                { "C2", 1.1m },
                { "C3", 1.15m },
                { "D1", 1.2m },
                { "D2", 1.3m },
                { "D3", 1.4m },
                { "D4", 1.5m },
                { "N", 1.6m }
            };

            // Находит коэффициент для вычисления эталонного времени трассы, 
            // соответствующий найденному ранее классу соревнования и лучшему времени в этом классе.
            // и умножает лучшее время на коэффициент для получения эталонного времени.
            decimal coeMax = default;
            foreach (var coe in coefficients)
            {
                if (coe.Key == bestClass)
                {
                    coeMax = coe.Value;
                    bestTime = SetClassTime(bestTime, coeMax);
                }
            }

            while (true)
            {
                var coeNew = coefficients.FirstOrDefault(c => c.Value > coeMax).Value;
                var classNew = coefficients.FirstOrDefault(c => c.Value == coeNew).Key;
                if (coeNew != default)
                {
                    foreach (var rider in riderController.Riders)
                    {
                        if (string.Compare(rider.ResultClassId, classNew) > 0 && rider.BestResult < IncreaseClassTime(bestTime, coeNew))
                        {
                            rider.ResultClassId = classNew;
                        }
                    }
                }
                else
                {
                    break;
                }
                coeMax = coeNew;
            }
            riderController.Save();
        }


        /// <summary>
        /// Находит самый высокий класс, в котором есть три участника.
        /// </summary>
        /// <param name="riderController"> Контроллер участника. </param>
        /// <param name="bestClass"> Изначально пустое значение максимального класса, в котором есть трое участников. </param>
        /// <returns> Название максимального класса, в котором есть трое участников. </returns>
        private static string SetCompetitionClass(RiderController riderController, string bestClass)
        {
            var count = 0;
            for (int i = 1; i < riderController.Riders.Count; i++)
            {
                if (riderController.Riders[i].PreviousClassId == riderController.Riders[i - 1].PreviousClassId)
                {
                    count++;
                    if (count.Equals(2))
                    {
                        bestClass = riderController.Riders[i].PreviousClassId;
                        return bestClass;
                    }
                }
                else
                {
                    count = 0;
                }
            }
            return bestClass;
        }

        /// <summary>
        /// Рассчёт лучшего времени для класса и округление в int. Принимает лучшеее время участника 
        /// в классе соревнования, делит его на коэффициент класса участника, возвращает
        /// эталонное время трассы.
        /// </summary>
        /// <param name="time"> Лучшее время участника в классе соревнования. </param>
        /// <param name="coe"> Коэффициент для рассчёта эталонного времени для класса соревнования. </param>
        /// <returns> Эталонное время трассы. </returns>
        private static int SetClassTime(int time, decimal coe)
        {
            decimal timeDecimal = time;
            timeDecimal /= coe;
            return Decimal.ToInt32(Math.Round(timeDecimal, MidpointRounding.AwayFromZero));
        }

        /// <summary>
        /// Рассчет эталонного времени для текущего класса, умножением эталонного времени трассы 
        /// на коэффициент текущего класса.
        /// </summary>
        /// <param name="time"> Эталонное время трассы. </param>
        /// <param name="coe"> Коэффициент для вычисления эталонного времени текущего класса. </param>
        /// <returns> Эталонное время для текущего класса. </returns>
        private static int IncreaseClassTime(int time, decimal coe)
        {
            decimal timeDecimal = time;
            timeDecimal *= coe;
            return Decimal.ToInt32(Math.Round(timeDecimal, MidpointRounding.AwayFromZero));
        }
    }
}
