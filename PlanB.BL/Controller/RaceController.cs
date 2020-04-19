using PlanB.BL.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlanB.BL.Controller
{
    public static class RaceController
    {
        /// <summary>
        /// Устанавливает результат заезда.
        /// </summary>
        /// <param name="riderController"> Контроллер участника. </param>
        /// <param name="lapTime"> Время круга в сотых. </param>
        /// <param name="penalty"> Штрафные баллы в сотых долях секунды. </param>
        /// <param name="newTryFirst"> При значении 1 перезаписывает результат первой попытки. </param>
        /// <param name="newTrySecond"> При значении 1 перезаписывает результат второй попытки. </param>
        /// <param name="infinity"> Если значение 1 перезаписывать новый результат вместо худшего, 0 - после заполнения двух попыток не записывать больше. </param>
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
                total = Rider.MAXTIME;
            }

            // Если нет дополнительных требований к перезаписи результатов, просто две попытки.
            // Eсли установлено строго две попытки, то при третьей и более попыток результат будет игнорироваться.
            if (infinity == false && newTryFirst == false && newTrySecond == false)
            {
                SetResults(rider, total);
            }
            else
            {
                // если требуется перезаписать результат одной из попыток.
                if (newTryFirst == true || newTrySecond == true)
                {
                    ReTry(rider, total, newTryFirst);
                }

                // если установлена бесконечная перезапись худшей попытки на лучшую.
                if (infinity == true && newTryFirst == false && newTrySecond == false)
                {
                    IfInfinity(rider, total);
                }
            }
            SetBestResult(rider);
        }

        /// <summary>
        /// Коректирует позицию участника исходя из его лучшего результата.
        /// </summary>
        /// <param name="riderController"> Контроллер участника. </param>
        public static void SetNewPlaces(RiderController riderController)
        {
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
        /// Установить лучшее время сравнением результатов двух попыток.
        /// </summary>
        /// <param name="rider"> Текущий участник. </param>
        private static void SetBestResult(Rider rider)
        {
            if ((rider.TryFirst <= rider.TrySecond
                && rider.TryFirst > 0) || (rider.TrySecond.Equals(0) && rider.TryFirst > 0))
            {
                rider.BestResult = rider.TryFirst;
            }
            else if (rider.TrySecond != 0)
            {
                rider.BestResult = rider.TrySecond;
            }
            else
            {
                rider.BestResult = Rider.MAXTIME;
            }
        }

        /// <summary>
        /// Перезаезд. Перезапись первого или второго результата в зависимотти от значений newTryFirst и newTrySecond.
        /// Ну или почти. Т.к. этот метод будет вызван если хотя бы одно из указанных значений установлено в 1, то
        /// newTrySecond проверять не имеет особого смысла.
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
        /// Запись результата заезда в свободный слот. Если оба заняты, то никуда.
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
        /// Если установлен флаг бесконечной перезаписи лучшего нового результата, перезаписывает его, если он лучший.
        /// </summary>
        /// <param name="rider"> Текущий участник. </param>
        /// <param name="total"> Итоговое время заезда. </param>
        private static void IfInfinity(Rider rider, int total)
        {
            if(total > 0)
            {
                if ((rider.TryFirst >= rider.TrySecond && rider.TrySecond != 0 && rider.TryFirst > total && total > 0) || rider.TryFirst == 0)
                {
                    rider.TryFirst = total;
                }
                else if ((rider.TrySecond > total && total > 0) || rider.TrySecond == 0)
                {
                    rider.TrySecond = total;
                }
            }
        }

        /// <summary>
        /// Определяет класс соревнования, если в текущем классе 3 участника и более, иначе
        /// возвращает null. Если класс находится, то изменяет bestTime на время лучшего участника 
        /// в классе соревнования. 
        /// </summary>
        /// <param name="riderController"> Контроллер участника. </param>
        /// <param name="bestTime"> Время лучшего участника в классе соревнования. </param>
        /// <returns></returns>
        public static string FindCompetitionClassId(RiderController riderController, ref int bestTime)
        {
            if (riderController is null)
            {
                throw new ArgumentNullException("Rider Controller cannot be null.", nameof(riderController));
            }

            // bestClass - класс соревнования.
            var bestClass = string.Empty;
            bestTime = 0;
            bestClass = SetCompetitionClass(riderController);
            if (string.IsNullOrEmpty(bestClass))
            {
                return null;
            }

            // Находит лучшее время среди участников в классе соревнования.
            bestTime = riderController.Riders.FirstOrDefault(r => r.PreviousClassId == bestClass).BestResult;
            if(bestTime == 0)
            {
                throw new ArgumentException("Best time cannot be set.", nameof(bestTime));
            }
            return bestClass;
        }

        /// <summary>
        /// Установка новых классов участников по результатам соревнования.
        /// Сравнивает лучшее время участника с эталонным для класса и присваевает новый класс, 
        /// если время участника лучше.
        /// </summary>
        /// <param name="riderController"> Контроллеp участника. </param>
        /// <param name="bestClass"> Класс соревнования. </param>
        /// <param name="bestTime"> Время лучшего участника в классе соревнования. </param>
        public static void SetNewClasses(RiderController riderController, string bestClass, int bestTime)
        {
            if (riderController is null)
            {
                throw new ArgumentNullException("Rider Controller cannot be null.", nameof(riderController));
            }

            if (bestClass is null)
            {
                throw new ArgumentNullException("Best class cannot be null.", nameof(bestClass));
            }

            if (bestTime <= 0)
            {
                throw new ArgumentNullException("Best time have to be positive.", nameof(bestTime));
            }

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
            // и делит лучшее время на коэффициент для получения эталонного времени.
            decimal coeMax = default;
            foreach (var coe in coefficients)
            {
                if (coe.Key == bestClass)
                {
                    coeMax = coe.Value;
                    // определяет эталонное время для класса, делением на коэффициент из таблицы.
                    bestTime = SetClassTime(bestTime, coeMax);
                    break;
                }
            }

            while (true)
            {
                decimal coeNew = default;
                string classNew = default;

                var classResult = coefficients.FirstOrDefault(c => c.Value == coeMax).Key;
                if (!classResult.Contains("N"))
                {
                    coeNew = coefficients.FirstOrDefault(c => c.Value > coeMax).Value;
                    classNew = coefficients.FirstOrDefault(c => c.Value == coeNew).Key;
                }
                else 
                {
                    break;                
                }
                
                
                if (coeNew != default)
                {
                    foreach (var rider in riderController.Riders)
                    {
                        if (string.Compare(rider.ResultClassId, classNew) >= 0 && rider.BestResult < IncreaseClassTime(bestTime, coeNew))
                        {
                            // так как перебор со старшего класса вниз, чтобы не присвоить младший класс, 
                            // которому результат так же будет соответствовать, проверка был ли ранее присвоен класс.
                            if(rider.PreviousClassId == rider.ResultClassId)
                            {
                                rider.ResultClassId = classResult;
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Break");
                    break;
                }
                coeMax = coeNew;
            }
            riderController.Save();
        }


        /// <summary>
        /// Находит самый высокий класс, в котором есть три участника. Если не находит, возвращает null.
        /// </summary>
        /// <param name="riderController"> Контроллер участника. </param>
        /// <returns> Название максимального класса, в котором есть трое участников. </returns>
        private static string SetCompetitionClass(RiderController riderController)
        {
            if (riderController is null || riderController.Riders == null)
            {
                throw new ArgumentNullException("Rider controller cannot be null.",  nameof(riderController));
            }

            int count;
            string bestClass;
            var Classes = new List<string>();
            foreach(var r in riderController.Riders)
            {
                Classes.Add(r.PreviousClassId);
            }
            Classes.Sort();
            count = 0;
            for(int i = 1; i < Classes.Count; i++)
            {
                if(Classes[i] == Classes[i - 1])
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
            return null;
        }

        /// <summary>
        /// Рассчёт лучшего времени для класса и округление в int. Принимает лучшеее время участника 
        /// в классе соревнования, делит его на коэффициент класса участника, возвращает
        /// эталонное время трассы.
        /// </summary>
        /// <param name="time"> Лучшее время участника в классе соревнования. </param>
        /// <param name="coe"> Коэффициент рассчёта эталонного времени для класса соревнования. </param>
        /// <returns> Эталонное время трассы. </returns>
        private static int SetClassTime(int time, decimal coe)
        {
            if (time <= 0)
            {
                throw new ArgumentException("Time have to be positive.", nameof(time));
            }
            if (coe <= 0)
            {
                throw new ArgumentException("Coefficient have to be positive.", nameof(coe));
            }

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
            if (time <= 0)
            {
                throw new ArgumentException("Time have to be positive.", nameof(time));
            }
            if (coe <= 0)
            {
                throw new ArgumentException("Coefficient have to be positive.", nameof(coe));
            }

            decimal timeDecimal = time;
            timeDecimal *= coe;
            return Decimal.ToInt32(Math.Round(timeDecimal, MidpointRounding.AwayFromZero));
        }
    }
}
