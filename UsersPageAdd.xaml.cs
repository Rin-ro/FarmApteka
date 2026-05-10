using System;
using System.Windows;
using AptekaLib;
using Apteka.Services;

namespace Apteka
{
    public partial class UsersPageAdd : Window
    {
        private readonly ApiService _api = App.Api;
        public User? EditedUser { get; private set; }

        public UsersPageAdd() { InitializeComponent(); }
        public UsersPageAdd(User user) : this() { EditedUser = user; LogTB.Text = user.Login; FIOTB.Text = user.FIO; EmailOrPhoneTB.Text = user.EmailOrPhone; PassTB.Text = ""; }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LogTB.Text) || string.IsNullOrWhiteSpace(PassTB.Text)) { MessageBox.Show("Логин и пароль обязательны"); return; }
            try
            {
                var user = new User { Login = LogTB.Text.Trim(), PasswordHash = PassTB.Text, FIO = FIOTB.Text.Trim(), EmailOrPhone = EmailOrPhoneTB.Text.Trim() };
                bool ok = EditedUser == null ? await _api.RegisterAsync(user) : await _api.UpdateUserAsync(new User { Id = EditedUser.Id, Login = user.Login, PasswordHash = user.PasswordHash, FIO = user.FIO, EmailOrPhone = user.EmailOrPhone });
                if (ok) DialogResult = true;
                else MessageBox.Show("Ошибка сохранения");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void RegBtn_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}