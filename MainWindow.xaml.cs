using System;
using System.Windows;
using System.Windows.Controls;
using AptekaLib;
using Apteka.Services;

namespace Apteka
{
    public partial class MainWindow : Window
    {
        private Button[] _navButtons;
        private readonly ApiService _api = App.Api;

        public MainWindow()
        {
            InitializeComponent();
            _navButtons = new[] { UsersBtn, MedicinesBtn, CategoriesBtn, OrdersBtn, OrderItemsBtn };
            NavigateToPage("Medicines");
        }

        private void NavBtn_Click(object sender, RoutedEventArgs e) { if (sender is Button btn && btn.Tag is string tag) NavigateToPage(tag); }
        private void BtnHome_Click(object sender, RoutedEventArgs e) => NavigateToPage("Medicines");

        private void NavigateToPage(string tag)
        {
            foreach (var btn in _navButtons) btn.Style = (Style)FindResource("NavButtonStyle");
            switch (tag)
            {
                case "Users": MainFrame.Navigate(new UsersPage()); UsersBtn.Style = (Style)FindResource("ActiveNavButtonStyle"); break;
                case "Medicines": MainFrame.Navigate(new MedicinesPage()); MedicinesBtn.Style = (Style)FindResource("ActiveNavButtonStyle"); break;
                case "Categories": MainFrame.Navigate(new CategoriesPage()); CategoriesBtn.Style = (Style)FindResource("ActiveNavButtonStyle"); break;
                case "Orders": MainFrame.Navigate(new OrdersPage()); OrdersBtn.Style = (Style)FindResource("ActiveNavButtonStyle"); break;
                case "OrderItems": MainFrame.Navigate(new OrdersPositionsPage()); OrderItemsBtn.Style = (Style)FindResource("ActiveNavButtonStyle"); break;
            }
            Title = $"Аптека «Пилюля» — {tag}";
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Window? w = MainFrame.Content switch
            {
                UsersPage => new UsersPageAdd(),
                MedicinesPage => new MedicinesPageAdd(),
                CategoriesPage => new CategoriesPageAdd(),
                OrdersPage => new OrdersPageAdd(),
                OrdersPositionsPage => new OrdersPositionsPageAdd(),
                NotificationsPage => new NotificationsPageAdd(),
                _ => null
            };
            if (w?.ShowDialog() == true) RefreshCurrentPage();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            object? sel = GetSelectedItemFromPage();
            if (sel == null) { MessageBox.Show("Выберите запись"); return; }

            Window? w = sel switch
            {
                User user => new UsersPageAdd(user),
                Medicine med => new MedicinesPageAdd(med),
                Category cat => new CategoriesPageAdd(cat),
                Order order => new OrderStatusEdit(order.Id, order.Status),  // редактируем только статус
                OrderPosition pos => new OrdersPositionsPageAdd(pos),
                Notification notif => new NotificationsPageAdd(notif),
                _ => null
            };

            if (w?.ShowDialog() == true) RefreshCurrentPage();
            else if (w == null) MessageBox.Show("Редактирование не поддерживается");
        }

        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            object? sel = GetSelectedItemFromPage();
            if (sel == null) { MessageBox.Show("Выберите запись"); return; }
            if (MessageBox.Show("Удалить?", "Подтверждение", MessageBoxButton.YesNo) != MessageBoxResult.Yes) return;

            bool ok = false;
            switch (sel)
            {
                case User user: ok = await _api.DeleteUserAsync(user.Id); break;
                case Medicine med: ok = await _api.DeleteMedicineAsync(med.Id); break;
                case Category cat: ok = await _api.DeleteCategoryAsync(cat.Id); break;
                case Order order: ok = await _api.CancelOrderAsync(order.Id); break;
                case OrderPosition pos: ok = await _api.DeleteOrderPositionAsync(pos.Id); break;
                case Notification notif: ok = await _api.DeleteNotificationAsync(notif.Id); break;
            }

            if (ok) RefreshCurrentPage();
            else MessageBox.Show("Ошибка удаления");
        }

        private object? GetSelectedItemFromPage()
        {
            if (MainFrame.Content is Page page)
            {
                var method = page.GetType().GetMethod("GetSelectedItem");
                return method?.Invoke(page, null);
            }
            return null;
        }

        private void RefreshCurrentPage()
        {
            if (MainFrame.Content is Page page)
            {
                var method = page.GetType().GetMethod("Refresh");
                method?.Invoke(page, null);
            }
        }
    }
}