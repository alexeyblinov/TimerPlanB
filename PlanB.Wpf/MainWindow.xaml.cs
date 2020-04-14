using PlanB.BL.Controller;
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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Main.Content = new Start();
        }

        private void MenuItemStart_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new Start();
        }

        private void MenuItemReg_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new Registration();
        }

        private void MenuItemSettings_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new Settings();
        }

        private void MenuItemResult_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new Result();
        }
    }
}
