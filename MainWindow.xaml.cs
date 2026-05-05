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

namespace Apteka
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new UsersPage());
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            this.Title = ((Page)e.Content).Title;
        }

        private void UsersBut_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new UsersPage());
        }

        private void MedicinesBut_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new MedicinesPage());
        }

        private void CategoriesBut_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CategoriesPage());
        }

        private void OrdersBut_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new OrdersPage());
        }

        private void OrdersPositionsBut_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new OrdersPositionsPage());
        }
    }
}
