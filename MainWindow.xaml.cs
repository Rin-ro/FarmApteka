using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
namespace Apteka
{
    public partial class MainWindow : Window
    {
        private Button[] _navButtons;
        public MainWindow()
        {
            InitializeComponent();
            _navButtons = new[] { UsersBtn, MedicinesBtn, CategoriesBtn, OrdersBtn, OrderItemsBtn };
            NavigateToPage("Users");
        }
        private void NavBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string pageTag)
            {
                NavigateToPage(pageTag);
            }
        }
        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPage("Users");
        }
        private void NavigateToPage(string pageTag)
        {
            foreach (var btn in _navButtons)
            {
                btn.Style = (Style)FindResource("NavButtonStyle");
            }
            switch (pageTag)
            {
                case "Users":
                    MainFrame.Navigate(new UsersPage());
                    UsersBtn.Style = (Style)FindResource("ActiveNavButtonStyle");
                    break;
                case "Medicines":
                    MainFrame.Navigate(new MedicinesPage());
                    MedicinesBtn.Style = (Style)FindResource("ActiveNavButtonStyle");
                    break;
                case "Categories":
                    MainFrame.Navigate(new CategoriesPage());
                    CategoriesBtn.Style = (Style)FindResource("ActiveNavButtonStyle");
                    break;
                case "Orders":
                    MainFrame.Navigate(new OrdersPage());
                    OrdersBtn.Style = (Style)FindResource("ActiveNavButtonStyle");
                    break;
                case "OrderItems":
                    MainFrame.Navigate(new OrdersPositionsPage());
                    OrderItemsBtn.Style = (Style)FindResource("ActiveNavButtonStyle");
                    break;
            }
            this.Title = $"Аптека «Пилюля» — {pageTag}";
        }
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Window addWindow = null;
            if (MainFrame.Content is UsersPage)
                addWindow = new UsersPageAdd();
            else if (MainFrame.Content is MedicinesPage)
                addWindow = new MedicinesPageAdd();
            else if (MainFrame.Content is CategoriesPage)
                addWindow = new CategoriesPageAdd();
            else if (MainFrame.Content is OrdersPage)
                addWindow = new OrdersPageAdd();
            else if (MainFrame.Content is OrdersPositionsPage)
                addWindow = new OrdersPositionsPageAdd();
            else
            {
                MessageBox.Show("Выберите раздел для добавления записи", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (addWindow.ShowDialog() == true)
            {
                RefreshCurrentPage();
                MessageBox.Show("Запись добавлена!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Выберите запись в таблице для редактирования", "Инфо",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Удалить выбранную запись?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                RefreshCurrentPage();
                MessageBox.Show("Запись удалена", "Готово",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void RefreshCurrentPage()
        {
            if (MainFrame.Content is Page currentPage)
            {
                var pageType = currentPage.GetType();
                var newPage = Activator.CreateInstance(pageType) as Page;
                MainFrame.Navigate(newPage);
            }
        }
    }
}