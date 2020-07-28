using PlanB.BL.Controller;
using PlanB.BL.Model;
using PlanB.Validators;
using System;
using System.Windows;
using System.Windows.Controls;
using FluentValidation;


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
        public RiderValidator riderValidator;

        public Registration()
        {
            InitializeComponent();
            riderController.Load();
        }
           
        /// <summary>
        /// Создать нового участника по стартовому номеру, если он уникальный и установить ему указанный класс.
        /// </summary>
        private void ClassList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ClassList.SelectedItem != null)
            {
                ComboBox comboBox = (ComboBox)sender;
                ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
                classId = selectedItem.Content.ToString();

                int.TryParse(StartNumberTextBox.Text, out startNunber);

                riderController = new RiderController(startNunber, classId);
                riderValidator = new RiderValidator();
                var validationResult = riderValidator.Validate(riderController.CurrentRider, ruleSet: "FirstRegistration");
                if (!validationResult.IsValid)
                {
                    foreach (var failure in validationResult.Errors)
                    {
                        MessageBox.Show(failure.ErrorMessage);
                    }
                    ClearRegistrationPage();
                    return;
                }

                if (!string.IsNullOrEmpty(riderController.CurrentRider.Name))
                {
                    StatusBarText(riderController);
                    MessageBox.Show("Участник с таким номером уже зарегистрирован.");
                    ClearRegistrationPage();
                }
            }
            
        }

        /// <summary>
        /// Заполнение поля пол на основании выбора пользователя.
        /// </summary>
        private void GenderList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(GenderList.SelectedItem != null)
            {
                ComboBox comboBox = (ComboBox)sender;
                ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
                gender = new Gender(selectedItem.Tag.ToString());
            }
            
        }

        /// <summary>
        /// Заполнение дополнительных данных об участнике, если он не был ранее зарегистрирован.
        /// </summary>
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

                try
                {
                    riderController.SetNewRiderData(name, surname, gender, location, team, isCruiser);
                }
                catch(ArgumentException ex)
                {
                    MessageBox.Show(ex.Message);
                    ClearRegistrationPage();
                }

                StatusBarText(riderController);
                ClearRegistrationPage();
            }


        }

        /// <summary>
        /// Сделать кнопку регистрации неактивной.
        /// </summary>
        private void StartNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            RegButton.IsEnabled = false;
        }

        /// <summary>
        /// При начале ввода в строку имени команды, вывести список существующих команд в статусбар.
        /// Если все поля заполнены, сделать кнопку регистрации активной.
        /// </summary>
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
                    RegButton.IsEnabled = false;
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

        /// <summary>
        /// Форматирование вывода информации об участнике в статусбар.
        /// </summary>
        /// <param name="riderController"></param>
        private void StatusBarText(RiderController riderController)
        {
            NameTextBox.Text = riderController.CurrentRider.Name;
            SurnameTextBox.Text = riderController.CurrentRider.Surname;
            LocationTextBox.Text = riderController.CurrentRider.Location;
            TeamTextBox.Text = riderController.CurrentRider.Team;
            StatusTextBlock.Text = riderController.CurrentRider.ToString();
        }
    }
}
