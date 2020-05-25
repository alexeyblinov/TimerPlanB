using PlanB.BL.Controller;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

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
                try
                {
                    bestClass = RaceController.FindCompetitionClassId(riderController, ref bestTime);
                }
                catch(ArgumentException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }
            // Рассчёт эталонного времени трассы и установка новых классов по результатам соревнования.
            riderController = RaceController.SetNewClasses(riderController, bestClass, bestTime);
        }

        private void ResultButton_Click(object sender, RoutedEventArgs e)
        {
            // коряво, конечно, но выглядит красивее, чем столбцы подписывать, поэтому пока надо подумать.
            if (ClassesList.SelectedIndex != 7 && ClassesList.SelectedIndex != 10)
            {
                TableHeaderTextBlock.Text = "Позиция                Лучшее время       Стартовый номер             Фамилия                    Класс                  Итоговый класс";
            }
            else
            {
                TableHeaderTextBlock.Text = "Позиция                                                Название команды                                    Количество очков";
            }
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
                    try
                    {
                        ResultTextBox.Document = new FlowDocument(RaceController.CreateTable(riderController));
                    }
                    catch (ArgumentException ex)
                    {
                        MessageBox.Show(ex.Message);
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
                    try
                    {
                        ResultTextBox.Document = new FlowDocument(RaceController.CreateTable(riderController));
                    }
                    catch (ArgumentException ex)
                    {
                        MessageBox.Show(ex.Message);
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
