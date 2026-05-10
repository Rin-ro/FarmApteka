using Apteka.Services;
using AptekaLib;
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
using System.Windows.Shapes;

namespace Apteka
{
    /// <summary>
    /// Логика взаимодействия для NotificationsPageAdd.xaml
    /// </summary>
    public partial class NotificationsPageAdd : Window
    {
        private readonly ApiService _api = App.Api;
        public Notification? EditedNotification { get; private set; }

        public NotificationsPageAdd()
        {
            InitializeComponent();
        }

        public NotificationsPageAdd(Notification notification) : this()
        {
            EditedNotification = notification;
            UserIdTB.Text = notification.UserId.ToString();
            MessageTB.Text = notification.Message;
            IsReadTB.Text = notification.IsRead ? "1" : "0";
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(UserIdTB.Text, out var userId))
            {
                MessageBox.Show("ID пользователя должен быть числом"); return;
            }

            var isRead = IsReadTB.Text == "1";
            var notif = new Notification
            {
                UserId = userId,
                Message = MessageTB.Text.Trim(),
                IsRead = isRead,
                CreatedDate = EditedNotification?.CreatedDate ?? DateTime.Now
            };

            if (EditedNotification != null) notif.Id = EditedNotification.Id;

            bool success = EditedNotification == null
                ? await _api.AddNotificationAsync(notif)
                : await _api.UpdateNotificationAsync(notif);

            if (success) DialogResult = true;
            else MessageBox.Show("Ошибка сохранения");
        }

        private void RegBtn_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
