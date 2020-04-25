using PlanB.BL.Controller;
using PlanB.BL.Model;
using System.Windows;
using System.Windows.Controls;


namespace PlanB.Wpf
{
    /// <summary>
    /// Страница регистрации участников.
    /// </summary>
    // Здесь пара-тройка лишних строк. Если успею, разберусь и уберу.
    public partial class Registration : Page
    {
        RiderController riderController = new RiderController();
        private int startNunber;
        private string classId;
        private Gender gender = new Gender("M");
        
        public Registration()
        {
            InitializeComponent();
            riderController.Load();
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
                    ClearRegistrationPage();
                }
                else
                {
                    riderController = new RiderController(startNunber, classId);
                    if (!string.IsNullOrEmpty(riderController.CurrentRider.Name))
                    {
                        StatusBarText(riderController);

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
                var isCruiser = (bool)IsCruiserCheckBox.IsChecked;
                riderController.SetNewRiderData(name, surname, gender, location, team, isCruiser);
                StatusBarText(riderController);
                ClearRegistrationPage();
            }


        }

        private void StartNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            RegButton.IsEnabled = false;
        }

        private void TeamTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(TeamStatusTextBlock.Text))
            {
                foreach (var team in RaceController.GetTeams(riderController))
                {
                    TeamStatusTextBlock.Text += team + " ";
                }
            }

            if (!string.IsNullOrWhiteSpace(StartNumberTextBox.Text) &&
                !string.IsNullOrWhiteSpace(NameTextBox.Text) &&
                !string.IsNullOrWhiteSpace(SurnameTextBox.Text) &&
                !string.IsNullOrWhiteSpace(LocationTextBox.Text) &&
                !string.IsNullOrWhiteSpace(TeamTextBox.Text))
            {
                RegButton.IsEnabled = true;
            }
            else 
            {
                if(RegButton.IsEnabled == true)
                {
                    MessageBox.Show("Не все поля заполнены.");
                }
                
            }
        }

        /// <summary>
        /// Очистка полей. Ничего лучше не придумал. Найду время, почитаю, переделаю.
        /// </summary>
        private void ClearRegistrationPage()
        {
            InitializeComponent();
            RegButton.IsEnabled = false;
            ClassList.SelectedItem = null;
            GenderList.SelectedItem = null;
            StartNumberTextBox.Text = null;
            NameTextBox.Text = null;
            SurnameTextBox.Text = null;
            LocationTextBox.Text = null;
            TeamTextBox.Text = null;
            TeamStatusTextBlock.Text = string.Empty;
            IsCruiserCheckBox.IsChecked = false;
        }

        private void StatusBarText(RiderController riderController)
        {
            NameTextBox.Text = riderController.CurrentRider.Name;
            SurnameTextBox.Text = riderController.CurrentRider.Surname;
            LocationTextBox.Text = riderController.CurrentRider.Location;
            TeamTextBox.Text = riderController.CurrentRider.Team;
            var result = string.Concat("#", riderController.CurrentRider.RiderId, " ",
                                       riderController.CurrentRider.Name, " ",
                                       riderController.CurrentRider.Surname, " [",
                                       riderController.CurrentRider.PreviousClassId, "]");
            StatusTextBlock.Text = result;
        }
    }
}
