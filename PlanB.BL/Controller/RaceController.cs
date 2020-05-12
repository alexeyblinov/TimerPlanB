using PlanB.BL.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Documents;

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
        public static RiderController SetNewClasses(RiderController riderController, string bestClass, int bestTime)
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

            // Если кто-то из зарегистрированных участников не проехал ни одной попытки, удалить их из списка.
            for(var i = 0; i < riderController.Riders.Count; i++)
            {
                if(riderController.Riders[i].TryFirst == Rider.MAXTIME && riderController.Riders[i].TrySecond == Rider.MAXTIME)
                {
                    riderController.Riders.RemoveAt(i);
                }
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
            return riderController;
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

        /// <summary>
        /// Установка очков команд в соответствии с занятыми участниками местами в классах награждения, кроме номинаций.
        /// </summary>
        /// <param name="riderController"> Контроллер участника. </param>
        /// <returns> Словарь, где ключ - название команды, значение - количество очков команды. </returns>
        public static Dictionary<string, int> SetTeamsRank(RiderController riderController, bool alternative = false)
        {
            // перераспределение мест в классе согласно классам награждения, вместо класса соревнования.
            int a= 1, i = 1, j = 1, k = 1;
            var ranks = new int[] { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 };
            ClearRanks(riderController);
            // если нужно считать очки в традиционной системе классов. Да, так коряво. Переделаю потом, как будет время.
            if(alternative == false)
            {
                foreach (var rider in riderController.Riders)
                {
                    switch (rider.PreviousClassId)
                    {
                        case "A":
                        case "B":
                        case "C1":
                        case "C2":
                            rider.Rank = a;
                            a++;
                            break;

                        case "C3":
                        case "D1":
                            rider.Rank = i;
                            i++;
                            break;

                        case "D2":
                        case "D3":
                            rider.Rank = j;
                            j++;
                            break;

                        case "D4":
                        case "N":
                            rider.Rank = k;
                            k++;
                            break;
                    }
                    if (rider.Rank < 11)
                    {
                        rider.Rank = ranks[rider.Rank - 1];
                    }
                    else
                    {
                        rider.Rank = 0;
                    }
                }
            }
            else
            {
                foreach (var rider in riderController.Riders)
                {
                    switch (rider.PreviousClassId)
                    {
                        case "A":
                        case "B":
                        case "C1":
                        case "C2":
                        case "C3":
                            rider.Rank = a;
                            a++;
                            break;

                        case "D1":
                            rider.Rank = i;
                            i++;
                            break;

                        case "D2":
                        case "D3":
                            rider.Rank = j;
                            j++;
                            break;

                        case "D4":
                        case "N":
                            rider.Rank = k;
                            k++;
                            break;
                    }
                    if (rider.Rank < 11)
                    {
                        rider.Rank = ranks[rider.Rank - 1];
                    }
                    else
                    {
                        rider.Rank = 0;
                    }
                }
            }

            // заполнениее словаря. Ключь - название команды, значение - суммерует места участников в классе награждения.
            var teams = new Dictionary<string, int>();
            foreach(var rider in riderController.Riders)
            {
                if(rider.Team != null)
                {
                    if (teams.Count == 0)
                    {
                        teams.Add(rider.Team, rider.Rank);
                    }
                    else
                    {
                        if (!teams.ContainsKey(rider.Team))
                        {
                            teams.Add(rider.Team, rider.Rank);
                        }
                        else
                        {
                            teams[rider.Team] += rider.Rank;
                        }
                    }
                }
            }
            teams = teams.OrderByDescending(t => t.Value).ToDictionary(t => t.Key, t => t.Value);

            return teams;
        }

        /// <summary>
        /// Возвращает список зарегистрированных команд.
        /// </summary>
        /// <param name="riderController"> Контроллер участника. </param>
        /// <returns> Список зарегистрированных команд. </returns>
        public static List<string> GetTeams(RiderController riderController)
        {
            var teams = new List<string>();
            foreach (var rider in riderController.Riders)
            {
                if (!teams.Contains(rider.Team))
                {
                    teams.Add(rider.Team);
                }
            }
            return teams;
        }

        /// <summary>
        /// Перезапись поля Rank для всех участников перед рассчётом очков для конкретной системы классов 
        /// (классической или альтернативной).
        /// </summary>
        /// <param name="riderController"> Контроллер участника. </param>
        private static void ClearRanks(RiderController riderController)
        {
            var i = 1;
            foreach(var rider in riderController.Riders)
            {
                rider.Rank = i;
                i++;
            }
        }

        public static Table CreateTable(List<Rider> riders, List<string> classes = null)
        {
            // если не указаны конкретные классы, вывести весь список участников.
            if(classes == null)
            {
                return MakeTable(riders);
            }
            // если указаны классы или номинации, вывести только участников указанных классов или номинаций.
            else
            {
                foreach (var c in classes)
                {
                    if (!Enum.IsDefined(typeof(ClassName), c) && c != "C" && c != "F")
                    {
                        throw new ArgumentException("Неверно указано название класса (идентификатор номинации).", nameof(c));
                    }
                }
                var result = new List<Rider>();
                
                // номинацию можно выводить только одну, либо C - круизёр, либо F - девушка.
                if (classes.Contains("C"))
                {
                    foreach(var rider in riders)
                    {
                        if (rider.IsCruiser)
                        {
                            result.Add(rider);
                        }
                    }
                    return MakeTable(result);
                }
                if (classes.Contains("F"))
                {
                    foreach (var rider in riders)
                    {
                        if (rider.Gender.Name == "F")
                        {
                            result.Add(rider);
                        }
                    }
                    return MakeTable(result);
                }
                // классов можно вывести сразу несколько, т.к. выводится класс награждения, который состоит из от 1 до 5 классов соревнования.
                foreach(var rider in riders)
                {
                    if (classes.Contains(rider.PreviousClassId))
                    {
                        result.Add(rider);
                    }
                }
                return MakeTable(result);
            }
            
            
        }


        /// <summary>
        /// Создаёт таблицу вывода результатов.
        /// </summary>
        /// <param name="riders"> Отсортированный список участников, результаты которых отобразятся в таблице. </param>
        /// <returns> Таблица результатов. </returns>
        private static Table MakeTable(List<Rider> riders)
        {
            if(riders.Count == 0)
            {
                throw new ArgumentException("Нет данных об участниках в текущем классе.");
            }
            var rows = riders.Count;
            var cols = 6;
            string[,] matrix = new string[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    // понятия не имею как сделать проще. Но я записался на халявный интернет семинар, где меня обещали научить.
                    // eсли после 15 мая я не исправил этот текст ниже, - значит не научили.
                    switch (j)
                    {
                        case 0:
                            matrix[i, j] = riders[i].Rank.ToString();
                            break;
                        case 1:
                            matrix[i, j] = TimemachineController.ToPrint(riders[i].BestResult);
                            break;
                        case 2:
                            matrix[i, j] = "#" + riders[i].RiderId;
                            break;
                        case 3:
                            matrix[i, j] = riders[i].Surname;
                            break;
                        case 4:
                            matrix[i, j] = "Класс: " + riders[i].PreviousClassId;
                            break;
                        case 5:
                            matrix[i, j] = "Kласс: " + riders[i].ResultClassId;
                            break;
                    }
                }
            }

            var table = new Table();
            for (int i = 0; i < cols; i++)
            {
                table.Columns.Add(new TableColumn());
            }
            var group = new TableRowGroup();
            table.RowGroups.Add(group);
            for (int i = 0; i < rows; i++)
            {
                var row = new TableRow();
                for (int j = 0; j < cols; j++)
                {
                    var background = System.Windows.Media.Brushes.White;
                    if (i % 2 == 0)
                    {
                        background = System.Windows.Media.Brushes.LightGray;
                    }
                    var cell = new TableCell(new Paragraph(new Run(matrix[i, j])))
                    {
                        Background = background,
                        IsEnabled = true,
                        TextAlignment = TextAlignment.Center
                    };
                    row.Cells.Add(cell);
                }
                group.Rows.Add(row);
            }
            return table;
        }
    }
}
