using Apteka.Services;
using AptekaLib;
using System;
using System.Windows;
using System.Windows.Controls;

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

        private void NavBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string pageTag)
                NavigateToPage(pageTag);
        }

        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPage("Medicines");
        }

        private void NavigateToPage(string pageTag)
        {
            foreach (var btn in _navButtons)
                btn.Style = (Style)FindResource("NavButtonStyle");

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
                NotificationsPage => new NotificationsPageAdd(),
                _ => null
            };

            if (addWindow?.ShowDialog() == true)
            {
                RefreshCurrentPage();
                MessageBox.Show("Запись добавлена!", "Успех");
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            object? selected = null;
            if (MainFrame.Content is Page page)
            {
                var method = page.GetType().GetMethod("GetSelectedItem");
                selected = method?.Invoke(page, null);
            }

            if (selected == null)
            {
                MessageBox.Show("Выберите запись для редактирования", "Инфо");
                return;
            }

            Window? editWindow = selected switch
            {
                User user => new UsersPageAdd(user),
                Medicine med => new MedicinesPageAdd(med),
                Category cat => new CategoriesPageAdd(cat),
                Order order => new OrdersPageAdd(),       // при необходимости передавайте order
                OrderPosition pos => new OrdersPositionsPageAdd(pos),
                Notification notif => new NotificationsPageAdd(notif),
                _ => null
            };

            if (editWindow?.ShowDialog() == true)
            {
                RefreshCurrentPage();
                MessageBox.Show("Запись обновлена!", "Успех");
            }
            else if (editWindow == null)
            {
                MessageBox.Show("Редактирование этого типа пока не поддерживается", "Инфо");
            }
        }

        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            object? selected = null;
            if (MainFrame.Content is Page page)
            {
                var method = page.GetType().GetMethod("GetSelectedItem");
                selected = method?.Invoke(page, null);
            }

            if (selected == null)
            {
                MessageBox.Show("Выберите запись для удаления", "Инфо");
                return;
            }

            var result = MessageBox.Show("Удалить выбранную запись?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;

            bool success = false;
            try
            {
                switch (selected)
                {
                    case User user: success = await _api.DeleteUserAsync(user.Id); break;
                    case Medicine med: success = await _api.DeleteMedicineAsync(med.Id); break;
                    case Category cat: success = await _api.DeleteCategoryAsync(cat.Id); break;
                    case Order order: success = await _api.CancelOrderAsync(order.Id); break;
                    case OrderPosition pos: success = await _api.DeleteOrderPositionAsync(pos.Id); break;
                    case Notification notif: success = await _api.DeleteNotificationAsync(notif.Id); break;
                    default:
                        var idProp = selected.GetType().GetProperty("Id");
                        if (idProp != null)
                        {
                            int id = (int)idProp.GetValue(selected);
                            // можно добавить доп. проверки типа, если нужно
                            success = await _api.DeleteNotificationAsync(id); // fallback
                        }
                        break;
                }

                if (success)
                {
                    RefreshCurrentPage();
                    MessageBox.Show("Запись удалена", "Готово");
                }
                else MessageBox.Show("Ошибка при удалении", "Ошибка");
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка: {ex.Message}"); }
        }

        private void RefreshCurrentPage()
        {
            if (MainFrame.Content is Page page)
            {
                var refreshMethod = page.GetType().GetMethod("Refresh");
                refreshMethod?.Invoke(page, null);
            }
        }
    }
}