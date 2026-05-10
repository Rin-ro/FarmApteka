using System;
using System.Windows;
using AptekaLib;
using Apteka.Services;

namespace Apteka
{
    public partial class NotificationsPageAdd : Window
    {
        private readonly ApiService _api = App.Api;
        public Notification? EditedNotification { get; private set; }

        public NotificationsPageAdd() { InitializeComponent(); }
        public NotificationsPageAdd(Notification notif) : this() { EditedNotification = notif; UserIdTB.Text = notif.UserId.ToString(); MessageTB.Text = notif.Message; IsReadTB.Text = notif.IsRead ? "1" : "0"; }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(UserIdTB.Text, out var uid)) { MessageBox.Show("ID пользователя число"); return; }
            var n = new Notification { UserId = uid, Message = MessageTB.Text.Trim(), IsRead = IsReadTB.Text == "1" };
            if (EditedNotification != null) n.Id = EditedNotification.Id;
            bool ok = EditedNotification == null ? await _api.AddNotificationAsync(n) : await _api.UpdateNotificationAsync(n);
            if (ok) DialogResult = true; else MessageBox.Show("Ошибка сохранения");
        }
        private void RegBtn_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}