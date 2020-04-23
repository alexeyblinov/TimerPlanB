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
            ResultTextBox.Text = string.Empty;

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
            RaceController.SetNewClasses(riderController, bestClass, bestTime);
        }

        private void ResultButton_Click(object sender, RoutedEventArgs e)
        {
            var i = 1;
            ResultTextBox.Text = string.Empty;
            switch (ClassesList.SelectedIndex)
            {
                case 0:
                    foreach(var rider in riderController.Riders)
                    {
                        PrintRider(i, rider);
                        i++;
                    }
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
                        ResultTextBox.Text = "Нет данных об участниках в текущем классе.";
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
                        ResultTextBox.Text = "Нет данных об участниках в текущем классе.";
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
                        ResultTextBox.Text = "Нет данных об участниках в текущем классе.";
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
                        ResultTextBox.Text = "Нет данных об участниках в текущем классе.";
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
                        ResultTextBox.Text = "Нет данных об участниках в текущем классе.";
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
                        ResultTextBox.Text = "Нет данных об участниках в текущем классе.";
                    }
                    break;
                case 7:
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
                        ResultTextBox.Text = "Нет данных об участниках в текущем классе.";
                    }
                    break;
                case 8:
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
                        ResultTextBox.Text = "Нет данных об участниках в текущем классе.";
                    }
                    break;
                case 9:
                    foreach (var rider in riderController.Riders)

                    if (i == 1)
                    {
                        ResultTextBox.Text = "Нет данных об участниках в текущем классе.";
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
            var bestResult = TimemachineController.ToPrint(rider.BestResult);
            var resultLine = string.Concat("Позиция: ", number, " Результат: ", bestResult, " #", rider.RiderId, " ", rider.Surname,
                " Класс: ", rider.PreviousClassId, " Итоговый класс: ", rider.ResultClassId, Environment.NewLine);
            ResultTextBox.Text += resultLine;
        }
    }
}
