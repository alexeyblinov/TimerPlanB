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
            TryAgainList.Tag = "0";
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            int.TryParse(StartNumberTextBox.Text, out int number);
            if(number > 0)
            {
                Rider rider = riderController.Riders.FirstOrDefault(r => r.RiderId.Equals(number));
                if (rider == null)
                {
                    MessageBox.Show("Участник с таким стартовым номером не зарегистрирован.");
                }
                else
                {
                    StatusBarTextBlock.Text = rider.Surname;
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
                StartNumberTextBox.Text = null;
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
                int.TryParse(StartNumberTextBox.Text, out int number);

                switch (TryAgainList.Tag.ToString())
                {
                    case "1":
                        if (OutOfRaceCheckBox.IsChecked == false)
                        {
                            TryTextBox.Text = "1";
                            RaceController.ChangeRank(riderController, riderController.Riders.FirstOrDefault(r => r.RiderId.Equals(number)), timeResult.HundredthsValue, penaltyResult, true);
                        }
                        break;
                    case "2":
                        if (OutOfRaceCheckBox.IsChecked == false)
                        {
                            TryTextBox.Text = "2";
                            RaceController.ChangeRank(riderController, riderController.Riders.FirstOrDefault(r => r.RiderId.Equals(number)), timeResult.HundredthsValue, penaltyResult, false, true);
                        }
                        break;
                    case "8":
                        if (OutOfRaceCheckBox.IsChecked == false)
                        {
                            TryTextBox.Text = "-";
                            RaceController.ChangeRank(riderController, riderController.Riders.FirstOrDefault(r => r.RiderId.Equals(number)), timeResult.HundredthsValue, penaltyResult, false, false, true);
                        }
                        break;
                    default:
                        if (OutOfRaceCheckBox.IsChecked == false)
                        {
                            RaceController.ChangeRank(riderController, riderController.Riders.FirstOrDefault(r => r.RiderId.Equals(number)), timeResult.HundredthsValue, penaltyResult);
                        }
                        break;
                }

                if (OutOfRaceCheckBox.IsChecked == true)
                {
                    if (TryTextBox.Text.Contains("1"))
                    {
                        riderController.Riders.FirstOrDefault(r => r.RiderId.Equals(number)).TryFirst = 0;
                    }
                    else
                    {
                        riderController.Riders.FirstOrDefault(r => r.RiderId.Equals(number)).TrySecond = 0;
                    }
                }
                    
            }
            StartNumberTextBox.Text = string.Empty;
            MinutesTextBox.Text = string.Empty;
            SecondsTextBox.Text = string.Empty;
            HundredthsTextBox.Text = string.Empty;
            PenaltyTextBox.Text = string.Empty;
            TryTextBox.Text = string.Empty;
            TryAgainList.Tag = "0";
            //TryAgainList.SelectedIndex = -1;
        }
    }
}
