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
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Page
    {
        RiderController riderController = new RiderController();
        private int startNunber;
        private string classId;
        private Gender gender = new Gender("M");
        
        public Registration()
        {
            InitializeComponent();
        }

        private void ClearRegistrationPage()
        {
            RegButton.IsEnabled = false;
            ClassList.SelectedItem = null;
            GenderList.SelectedItem = null;
            StartNumberTextBox.Text = null;
            NameTextBox.Text = null;
            SurnameTextBox.Text = null;
            LocationTextBox.Text = null;
            TeamTextBox.Text = null;
        }

        private void ClassList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ClassList.SelectedItem != null)
            {
                ComboBox comboBox = (ComboBox)sender;
                ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
                classId = selectedItem.Content.ToString();

                int.TryParse(StartNumberTextBox.Text, out startNunber);
                if (startNunber <= 0)
                {
                    MessageBox.Show("Введите стартовый номер (число от 1 до 99).");
                }
                else
                {
                    riderController = new RiderController(startNunber, classId);
                    if (!string.IsNullOrEmpty(riderController.CurrentRider.Name))
                    {
                        NameTextBox.Text = riderController.CurrentRider.Name;
                        SurnameTextBox.Text = riderController.CurrentRider.Surname;
                        LocationTextBox.Text = riderController.CurrentRider.Location;
                        TeamTextBox.Text = riderController.CurrentRider.Team;
                        RegButton.IsEnabled = false;
                        MessageBox.Show("Участник с таким номером уже зарегистрирован.");
                        ClearRegistrationPage();
                    }
                }
            }
            
        }

        private void GenderList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(GenderList.SelectedItem != null)
            {
                ComboBox comboBox = (ComboBox)sender;
                ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
                gender = new Gender(selectedItem.Tag.ToString());
            }
            
        }

        private void RegButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(riderController.CurrentRider.Name))
            {
                var name = NameTextBox.Text;
                var surname = SurnameTextBox.Text;
                var gender = this.gender.Name;
                var location = LocationTextBox.Text;
                var team = TeamTextBox.Text;
                riderController.SetNewRiderData(name, surname, gender, location, team);

                ClearRegistrationPage();
            }


        }


        private void TeamTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!NameTextBox.Text.Equals(null) &&
                !SurnameTextBox.Text.Equals(null) &&
                !gender.Equals(null) &&
                !LocationTextBox.Text.Equals(null) &&
                !TeamTextBox.Text.Equals(null))
            {
                RegButton.IsEnabled = true;
            }
            else 
            {
                MessageBox.Show("Не все поля заполнены.");
            }
        }
    }
}
