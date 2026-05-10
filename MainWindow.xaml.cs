using Apteka.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Apteka
{
    public partial class MainWindow : Window
    {
        private Button[] _navButtons;

        public MainWindow()
        {
            InitializeComponent();

            // 🔹 ИНИЦИАЛИЗАЦИЯ МАССИВА КНОПОК
            _navButtons = new[] { UsersBtn, MedicinesBtn, CategoriesBtn, OrdersBtn, OrderItemsBtn };

            NavigateToPage("Medicines");
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
            NavigateToPage("Medicines");
        }

        private void NavigateToPage(string pageTag)
        {
            // Сброс стилей всех кнопок
            foreach (var btn in _navButtons)
            {
                btn.Style = (Style)FindResource("NavButtonStyle");
            }

            // Переход на страницу и подсветка кнопки
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

            Title = $"Аптека «Пилюля» — {pageTag}";
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Window? addWindow = MainFrame.Content switch
            {
                UsersPage => new UsersPageAdd(),
                MedicinesPage => new MedicinesPageAdd(),
                CategoriesPage => new CategoriesPageAdd(),
                OrdersPage => new OrdersPageAdd(),
                OrdersPositionsPage => new OrdersPositionsPageAdd(),
                _ => null
            };

            if (addWindow?.ShowDialog() == true)
            {
                RefreshCurrentPage();
                MessageBox.Show("Запись добавлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Выберите запись для редактирования", "Инфо", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Удалить выбранную запись?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                RefreshCurrentPage();
                MessageBox.Show("Запись удалена", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
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