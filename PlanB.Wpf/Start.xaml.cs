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
using System.Windows.Shapes;

namespace PlanB.Wpf
{
    /// <summary>
    /// Логика взаимодействия для Start.xaml
    /// </summary>
    public partial class Start : Page
    {
        RiderController riderController = new RiderController();

        public Start()
        {
            InitializeComponent();
            riderController.Load();
            TryAgainList.SelectedIndex = 0;
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            int.TryParse(StartNumberTextBox.Text, out int number);
            if (number > 0)
            {
                Rider rider = ThisRider();
                if (rider != null)
                {
                    StatusBarTextBlock.Text = string.Concat(rider.RiderId, ": ", rider.Surname);
                    Try1ResultTextBox.Text = TimemachineController.ToPrint(rider.TryFirst);
                    Try2ResultTextBox.Text = TimemachineController.ToPrint(rider.TrySecond);
                    if (rider.TryFirst.Equals(Rider.MAXTIME))
                    {
                        TryTextBox.Text = "1";
                    }
                    else if (rider.TrySecond.Equals(Rider.MAXTIME))
                    {
                        TryTextBox.Text = "2";
                    }
                    else
                    {
                        TryTextBox.Text = "-";
                        MessageBox.Show("Обе попытки использованы.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Стартовый номер должен быть положительным числом от 1 до 99.");
                StartNumberTextBox.Text = string.Empty;
            }
            
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            
            int.TryParse(MinutesTextBox.Text, out int minutes);
            int.TryParse(SecondsTextBox.Text, out int seconds);
            int.TryParse(HundredthsTextBox.Text, out int hundredths);
            int.TryParse(PenaltyTextBox.Text, out int penalty);

            if (minutes < 0 || seconds < 0 || hundredths < 0)
            {
                MessageBox.Show("Неверное значение одного из полей");
                return;
            }
            else
            {
                var timeResult = new TimemachineController(minutes, seconds, hundredths);
                var penaltyResult = penalty * 100;
                
                switch (TryAgainList.SelectedIndex)
                {
                case 1:
                    if (OutOfRaceCheckBox.IsChecked == false)
                    {
                        TryTextBox.Text = "1";
                        RaceController.ChangeRank(riderController, ThisRider(), timeResult.HundredthsValue, penaltyResult, true);
                        StatusPrint();
                        }
                    break;
                case 2:
                    if (OutOfRaceCheckBox.IsChecked == false)
                    {
                        TryTextBox.Text = "2";
                        RaceController.ChangeRank(riderController, ThisRider(), timeResult.HundredthsValue, penaltyResult, false, true);
                        StatusPrint();
                        }
                    break;
                case 3:
                    if (OutOfRaceCheckBox.IsChecked == false)
                    {
                        TryTextBox.Text = "-";
                        RaceController.ChangeRank(riderController, ThisRider(), timeResult.HundredthsValue, penaltyResult, false, false, true);
                        StatusPrint();
                        }
                    break;
                default:
                    if (OutOfRaceCheckBox.IsChecked == false)
                    {
                        RaceController.ChangeRank(riderController, ThisRider(), timeResult.HundredthsValue, penaltyResult);
                        StatusPrint();
                    }
                    break;
                }

                if (OutOfRaceCheckBox.IsChecked == true)
                {
                    if (TryTextBox.Text.Contains("1"))
                    {
                        ThisRider().TryFirst = 0;
                        StatusPrint();
                    }
                    else
                    {
                        ThisRider().TrySecond = 0;
                        StatusPrint();
                    }
                }      
            }

            StartNumberTextBox.Text = string.Empty;
            MinutesTextBox.Text = string.Empty;
            SecondsTextBox.Text = string.Empty;
            HundredthsTextBox.Text = string.Empty;
            PenaltyTextBox.Text = string.Empty;
            TryTextBox.Text = string.Empty;
            OutOfRaceCheckBox.IsChecked = false;
            TryAgainList.SelectedIndex = 0;
            riderController.Save();
        }

        /// <summary>
        /// Вывод результата заезда в статусбары первой и второй попытки. 
        /// На основании номера попытки выбирает значение какой из строк в статусбаре изменить.
        /// </summary>
        private void StatusPrint()
        {
            int.TryParse(StartNumberTextBox.Text, out int number);
            if (number <= 0)
            {
                throw new ArgumentOutOfRangeException("Rider number have to be positive.", nameof(number));
            }
            if (TryTextBox.Text.Contains("1"))
            {
                var printResult = ThisRider().TryFirst;
                Try1ResultTextBox.Text = TimemachineController.ToPrint(printResult);
            }
            else if (TryTextBox.Text.Contains("2"))
            {
                var printResult = ThisRider().TrySecond;
                Try2ResultTextBox.Text = TimemachineController.ToPrint(printResult);
            }
            else
            {
                var printResult1 = ThisRider().TryFirst;
                Try1ResultTextBox.Text = TimemachineController.ToPrint(printResult1);
                var printResult2 = ThisRider().TrySecond;
                Try2ResultTextBox.Text = TimemachineController.ToPrint(printResult2);
            }
        }

        /// <summary>
        /// Возвращает текущего участника.
        /// </summary>
        /// <returns> Текущий участник </returns>
        private Rider ThisRider()
        {
            int.TryParse(StartNumberTextBox.Text, out int number);
            if(number <= 0)
            {
                throw new ArgumentOutOfRangeException("Rider's number have to be positive.", nameof(number));
            }
            var result = riderController.Riders.FirstOrDefault(r => r.RiderId.Equals(number));
            if (result == null)
            {
                MessageBox.Show("Участник с таким номером не найден.");
            }
            return result;
        }
    }
}
