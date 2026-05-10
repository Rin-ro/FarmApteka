using Apteka.Services;
using AptekaLib;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Apteka
{
    public partial class NotificationsPage : Page
    {
        private readonly ApiService _api = App.Api;
        public ObservableCollection<Notification> Notifications { get; } = new();

        public NotificationsPage()
        {
            InitializeComponent();
            DataContext = this;
            Loaded += (s, e) => LoadNotifications();
        }

        private async void LoadNotifications()
        {
            if (App.CurrentUser == null) return;
            var list = await _api.GetUserNotificationsAsync(App.CurrentUser.Id);
            Notifications.Clear();
            foreach (var n in list) Notifications.Add(n);
        }

        public object? GetSelectedItem() => notificationsDataTable.SelectedItem;
        public void Refresh() => LoadNotifications();
    }
}