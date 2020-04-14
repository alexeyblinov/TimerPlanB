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
            switch (TryAgainList.Tag.ToString())
            {
                case "1":

                    break;
                case "2":

                    break;
                case "8":

                    break;
                default:
                    if(OutOfRaceCheckBox.IsChecked == false)
                    {
                        int.TryParse(MinutesTextBox.Text, out int minutes);
                        int.TryParse(SecondsTextBox.Text, out int seconds);
                        int.TryParse(HundredthsTextBox.Text, out int hundredths);
                        int.TryParse(PenaltyTextBox.Text, out int penalty);
                        var timeRusult = new TimemachineController(minutes, seconds, hundredths);
                    
                    
                    }
                    else
                    {
                        riderController.Riders.FirstOrDefault(r => r.RiderId == )
                    }
                    

                    break;
            }
        }
    }
}
