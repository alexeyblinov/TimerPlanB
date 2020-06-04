using PlanB.BL.Controller;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;

namespace PlanB.BL.Model
{
    public class ResultTable
    {
        /// <summary>
        /// Возвращает таблицу участников.
        /// </summary>
        public virtual Table GetTable { get; }

        /// <summary>
        /// Создать таблицу участников.
        /// </summary>
        /// <param name="rows"> Количество строк. </param>
        /// <param name="cols"> Количество столбцов. </param>
        public ResultTable(List<Rider> riders)
        {
            if(riders.Count == 0)
            {
                throw new ArgumentException("Список участников пуст.", nameof(riders));
            }
            var rows = riders.Count;
            var cols = 11;

            string[,] matrix = new string[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    // понятия не имею как сделать проще. Но я записался на халявный интернет семинар, где меня обещали научить.
                    // eсли после 15 мая я не исправил этот текст ниже, - значит не научили. P.S. ещё не посмотрел.
                    switch (j)
                    {
                        case 0:
                            matrix[i, j] = riders[i].RiderId.ToString();
                            break;
                        case 1:
                            matrix[i, j] = riders[i].Name;
                            break;
                        case 2:
                            matrix[i, j] = riders[i].Surname;
                            break;
                        case 3:
                            if(riders[i].Gender.Name == "F")
                            {
                                matrix[i, j] = "Ж";
                            }
                            else
                            {
                                matrix[i, j] = "M";
                            }
                            break;
                        case 4:
                            matrix[i, j] = riders[i].Location;
                            break;
                        case 5:
                            matrix[i, j] = riders[i].Team;
                            break;
                        case 6:
                            matrix[i, j] = TimemachineController.ToPrint(riders[i].TryFirst);
                            break;
                        case 7:
                            matrix[i, j] = TimemachineController.ToPrint(riders[i].TrySecond);
                            break;
                        case 8:
                            matrix[i, j] = TimemachineController.ToPrint(riders[i].BestResult);
                            break;
                        case 9:
                            matrix[i, j] = riders[i].PreviousClassId;
                            break;
                        case 10:
                            matrix[i, j] = riders[i].ResultClassId;
                            break;
                    }
                }
            }

            GetTable = new Table();
            for (int i = 0; i < cols; i++)
            {
                GetTable.Columns.Add(new TableColumn());
            }
            var group = new TableRowGroup();
            GetTable.RowGroups.Add(group);
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
        }
    }
}
