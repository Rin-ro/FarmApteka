using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using AptekaLib;
using Apteka.Services;

namespace Apteka
{
    public partial class NotificationsPage : Page
    {
        private readonly ApiService _api = App.Api;
        public NotificationsPage()
        {
            InitializeComponent();
            Loaded += (s, e) => LoadNotifications();
        }

        private async void LoadNotifications()
        {
            if (App.CurrentUser == null) return;

            try
            {
                var notifications = await _api.GetUserNotificationsAsync(App.CurrentUser.Id);

                // Преобразуем для отображения в DataGrid,
                // используя ReadDate для определения статуса
                notificationsDataTable.ItemsSource = notifications.Select(n => new
                {
                    n.Id,
                    n.Message,
                    n.CreatedDate,
                    ReadDate = n.ReadDate?.ToString("dd.MM.yyyy HH:mm") ?? "—",
                    IsRead = n.ReadDate.HasValue,                // вычисляемое
                    StatusText = n.ReadDate.HasValue ? "✓ Прочитано" : "○ Новое",
                    StatusColor = n.ReadDate.HasValue ? "#FF888888" : "#FF00AA00"
                }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void BtnMarkRead_Click(object sender, RoutedEventArgs e)
        {
            if (notificationsDataTable.SelectedItem == null)
            {
                MessageBox.Show("Выберите уведомление", "Инфо");
                return;
            }

            dynamic selected = notificationsDataTable.SelectedItem;

            try
            {
                await _api.MarkNotificationAsReadAsync(selected.Id);
                LoadNotifications();  // обновить список
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
            }
        }

        private async void BtnMarkUnread_Click(object sender, RoutedEventArgs e)
        {
            if (notificationsDataTable.SelectedItem == null) return;

            dynamic selected = notificationsDataTable.SelectedItem;

            try
            {
                await _api.MarkNotificationAsUnreadAsync(selected.Id);
                LoadNotifications();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
            }
        }

        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (notificationsDataTable.SelectedItem == null) return;

            dynamic selected = notificationsDataTable.SelectedItem;

            var result = MessageBox.Show("Удалить это уведомление?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await _api.DeleteNotificationAsync(selected.Id);
                    LoadNotifications();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
                }
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadNotifications();
        }
    }
}