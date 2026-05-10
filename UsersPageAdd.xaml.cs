using Apteka.Services;
using AptekaLib;
using System;
using System.Windows;

namespace Apteka
{
    public partial class UsersPageAdd : Window
    {
        private readonly ApiService _api = App.Api;
        public User? EditedUser { get; private set; }

        public UsersPageAdd()
        {
            InitializeComponent();
        }

        public UsersPageAdd(User user) : this()
        {
            EditedUser = user;
            LogTB.Text = user.Login;
            EmailOrPhoneTB.Text = user.EmailOrPhone;
            FIOTB.Text = user.FIO;
            PassTB.Text = "";
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LogTB.Text) || string.IsNullOrWhiteSpace(PassTB.Text))
            {
                MessageBox.Show("Логин и пароль обязательны", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (EditedUser == null)
                {
                    var newUser = new User
                    {
                        Login = LogTB.Text.Trim(),
                        PasswordHash = PassTB.Text, 
                        EmailOrPhone = EmailOrPhoneTB.Text.Trim(),
                        FIO = FIOTB.Text.Trim(),
                        DateOfCreate = DateTime.Now
                    };

                    var success = await _api.RegisterAsync(newUser);
                    if (success)
                        DialogResult = true;
                    else
                        MessageBox.Show("Ошибка при создании пользователя", "Ошибка");
                }
                else
                {
                    EditedUser.FIO = FIOTB.Text.Trim();
                    EditedUser.EmailOrPhone = EmailOrPhoneTB.Text.Trim();
                    if (!string.IsNullOrWhiteSpace(PassTB.Text))
                        EditedUser.PasswordHash = PassTB.Text;

                    var success = await _api.UpdateUserAsync(EditedUser);
                    if (success)
                        DialogResult = true;
                    else
                        MessageBox.Show("Ошибка при обновлении", "Ошибка");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
            }
        }

        private void RegBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}