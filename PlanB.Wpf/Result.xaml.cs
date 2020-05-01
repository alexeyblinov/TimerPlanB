using PlanB.BL.Controller;
using PlanB.BL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace PlanB.Wpf
{
    /// <summary>
    /// Логика взаимодействия для Result.xaml
    /// </summary>
    public partial class Result : Page
    {
        RiderController riderController = new RiderController();
        int bestTime = 0;
        string bestClass = null;

        public Result()
        {
            InitializeComponent();
            ClassesList.SelectedIndex = 0;
            riderController.Load();
            ResultTextBox.Document.Blocks.Clear();

            RaceController.SetNewPlaces(riderController);

            if (riderController != null)
            {
                // если не найдёт эталонный класс, вернёт bestClass = null.
                bestClass = RaceController.FindCompetitionClassId(riderController, ref bestTime);
                if (bestClass == null)
                {
                    MessageBox.Show("Нет трёх участников ни в одном из классов соревнования. Определить класс соревнования невозможно.");
                    return;
                }
            }
            // Рассчёт эталонного времени трассы и установка новых классов по результатам соревнования.
            riderController = RaceController.SetNewClasses(riderController, bestClass, bestTime);
        }

        private void ResultButton_Click(object sender, RoutedEventArgs e)
        {
            var i = 1;
            ResultTextBox.Document.Blocks.Clear();
            switch (ClassesList.SelectedIndex)
            {
                case 0:
                    ResultTextBox.Document = new FlowDocument(RaceController.MakeTable(riderController.Riders));
                    //foreach(var rider in riderController.Riders)
                    //{
                    //    PrintRider(i, rider);
                    //    i++;
                    //}
                    break;
                case 1:
                    foreach (var rider in riderController.Riders)
                    {
                        if(rider.PreviousClassId == "A" || rider.PreviousClassId == "B" || 
                           rider.PreviousClassId == "C1" || rider.PreviousClassId == "C2")
                        {
                            PrintRider(i, rider);
                            i++;
                        }
                    }
                    if (i == 1)
                    {
                        ResultTextBox.AppendText("Нет данных об участниках в текущем классе.");
                    }
                    break;
                case 2:
                    foreach (var rider in riderController.Riders)
                    {
                        if (rider.PreviousClassId == "C3" || rider.PreviousClassId == "D1")
                        {
                            PrintRider(i, rider);
                            i++;
                        }
                    }
                    if (i == 1)
                    {
                        ResultTextBox.AppendText("Нет данных об участниках в текущем классе.");
                    }
                    break;
                case 3:
                    foreach (var rider in riderController.Riders)
                    {
                        if (rider.PreviousClassId == "D2" || rider.PreviousClassId == "D3")
                        {
                            PrintRider(i, rider);
                            i++;
                        }
                    }
                    if (i == 1)
                    {
                        ResultTextBox.AppendText("Нет данных об участниках в текущем классе.");
                    }
                    break;
                case 4:
                    foreach (var rider in riderController.Riders)
                    {
                        if (rider.PreviousClassId == "D4" || rider.PreviousClassId == "N")
                        {
                            PrintRider(i, rider);
                            i++;
                        }
                    }
                    if (i == 1)
                    {
                        ResultTextBox.AppendText("Нет данных об участниках в текущем классе.");
                    }
                    break;
                case 5:
                    foreach (var rider in riderController.Riders)
                    {
                        if (rider.IsCruiser)
                        {
                            PrintRider(i, rider);
                            i++;
                        }
                    }
                    if (i == 1)
                    {
                        ResultTextBox.AppendText("Нет данных об участниках в текущем классе.");
                    }
                    break;
                case 6:
                    foreach (var rider in riderController.Riders)
                    {
                        if (rider.Gender.Name == "F")
                        {
                            PrintRider(i, rider);
                            i++;
                        }
                    }
                    if (i == 1)
                    {
                        ResultTextBox.AppendText("Нет данных об участниках в текущем классе.");
                    }
                    break;
                case 7:
                    var teamsResult = RaceController.SetTeamsRank(riderController);
                    // записывается количество очков команды предыдущей итерации. 
                    var overlap = 0;
                    // записывается позиция команды предыдущей итерации.
                    var position = 0;
                    // было ли совпадение очков на предыдущей итерации.
                    var checkOverlap = false;
                    foreach (var team in teamsResult)
                    {
                        // если у команд одинаковое количество очков, они должны занимать одинаковое место,
                        // при этом следующая позиция пропускается.
                        if(team.Value == overlap)
                        {
                            i = position;
                            checkOverlap = true;
                        }
                        ResultTextBox.AppendText(Environment.NewLine);
                        var resultLine = string.Concat("Позиция: ", i, "  Команда: ", team.Key, ", количество очков: ", team.Value);
                        ResultTextBox.AppendText(resultLine);
                        overlap = team.Value;
                        position = i;
                        if(checkOverlap == true)
                        {
                            checkOverlap = false;
                        }
                        i++;
                    }
                    if (i == 1)
                    {
                        ResultTextBox.AppendText("Нет данных об участниках в текущем классе.");
                    }
                    break;
                case 8:
                    foreach (var rider in riderController.Riders)
                    {
                        if (rider.PreviousClassId == "A" || rider.PreviousClassId == "B" ||
                           rider.PreviousClassId == "C1" || rider.PreviousClassId == "C2" || rider.PreviousClassId == "C3")
                        {
                            PrintRider(i, rider);
                            i++;
                        }
                    }
                    if (i == 1)
                    {
                        ResultTextBox.AppendText("Нет данных об участниках в текущем классе.");
                    }
                    break;
                case 9:
                    foreach (var rider in riderController.Riders)
                    {
                        if (rider.PreviousClassId == "D1")
                        {
                            PrintRider(i, rider);
                            i++;
                        }
                    }
                    if (i == 1)
                    {
                        ResultTextBox.AppendText("Нет данных об участниках в текущем классе.");
                    }
                    break;
                case 10:
                    overlap = 0;
                    position = 0;
                    checkOverlap = false;
                    teamsResult = RaceController.SetTeamsRank(riderController, true);
                    foreach (var team in teamsResult)
                    {
                        if (team.Value == overlap)
                        {
                            i = position;
                            checkOverlap = true;
                        }
                        ResultTextBox.AppendText(Environment.NewLine);
                        var resultLine = string.Concat("Позиция: ", i, "  Команда: ", team.Key, ", количество очков: ", team.Value);
                        ResultTextBox.AppendText(resultLine);
                        overlap = team.Value;
                        position = i;
                        if (checkOverlap == true)
                        {
                            checkOverlap = false;
                        }
                        i++;
                    }
                    if (i == 1)
                    {
                        ResultTextBox.AppendText("Нет данных об участниках в текущем классе.");
                    }
                    break;
            }
        }

        /// <summary>
        /// Вывод данных об участнике в формате, подходящем для просмотра результатов соревнования.
        /// </summary>
        /// <param name="number"> Порядковый номер участника. </param>
        /// <param name="rider"> Участник, данные которого нужно вывести. </param>
        private void PrintRider(int number, Rider rider)
        {
            ResultTextBox.AppendText(Environment.NewLine);
            var bestResult = TimemachineController.ToPrint(rider.BestResult);
            var resultLine = string.Concat("Позиция: ", number, "  Результат: ", bestResult, "  #", rider.RiderId, "  ", rider.Surname,
                "  Класс: ", rider.PreviousClassId, "  Итоговый класс: ", rider.ResultClassId);
            ResultTextBox.AppendText(resultLine);
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog pd = new PrintDialog();
            if ((pd.ShowDialog() == true))
            {
                ResultTextBox.Document.PagePadding = new Thickness(50);
                ResultTextBox.Document.ColumnGap = 0;
                ResultTextBox.Document.ColumnWidth = pd.PrintableAreaWidth;
                pd.PrintDocument((((IDocumentPaginatorSource)ResultTextBox.Document).DocumentPaginator), "Печать результатов соревнования.");
                ResultTextBox.Document.PagePadding = new Thickness(0);
            }
        }
    }
}
