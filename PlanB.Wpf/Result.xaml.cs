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
                    try
                    {
                        ResultTextBox.Document = new FlowDocument(RaceController.CreateTable(riderController.Riders));
                    }
                    catch(ArgumentException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    break;
                case 1:
                    try
                    {
                        ResultTextBox.Document = new FlowDocument(RaceController.CreateTable(riderController.Riders, new List<string>() { "A", "B", "C1", "C2"}));
                    }
                    catch (ArgumentException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    break;
                case 2:
                    try
                    {
                        ResultTextBox.Document = new FlowDocument(RaceController.CreateTable(riderController.Riders, new List<string>() { "C3", "D1" }));
                    }
                    catch (ArgumentException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    break;
                case 3:
                    try
                    {
                        ResultTextBox.Document = new FlowDocument(RaceController.CreateTable(riderController.Riders, new List<string>() { "D2", "D3" }));
                    }
                    catch (ArgumentException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    break;
                case 4:
                    try
                    {
                        ResultTextBox.Document = new FlowDocument(RaceController.CreateTable(riderController.Riders, new List<string>() { "D4", "N" }));
                    }
                    catch (ArgumentException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    break;
                case 5:
                    try
                    {
                        ResultTextBox.Document = new FlowDocument(RaceController.CreateTable(riderController.Riders, new List<string>() { "C" }));
                    }
                    catch (ArgumentException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    break;
                case 6:
                    try
                    {
                        ResultTextBox.Document = new FlowDocument(RaceController.CreateTable(riderController.Riders, new List<string>() { "F" }));
                    }
                    catch (ArgumentException ex)
                    {
                        MessageBox.Show(ex.Message);
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
                    try
                    {
                        ResultTextBox.Document = new FlowDocument(RaceController.CreateTable(riderController.Riders, new List<string>() { "A", "B", "C1", "C2", "C3" }));
                    }
                    catch (ArgumentException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    break;
                case 9:
                    try
                    {
                        ResultTextBox.Document = new FlowDocument(RaceController.CreateTable(riderController.Riders, new List<string>() { "D1" }));
                    }
                    catch (ArgumentException ex)
                    {
                        MessageBox.Show(ex.Message);
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
        /// Распечатать результаты.
        /// </summary>
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
