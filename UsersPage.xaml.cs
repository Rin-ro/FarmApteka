using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using AptekaLib;
using Apteka.Services;

namespace Apteka
{
    public partial class UsersPage : Page
    {
        private readonly ApiService _api = App.Api;
        public ObservableCollection<User> Users { get; } = new();

        public UsersPage()
        {
            InitializeComponent();
            DataContext = this;
            LoadUsers();
        }

        private async void LoadUsers()
        {
            try
            {
                var users = await _api.GetUsersAsync();
                Users.Clear();
                foreach (var user in users)
                    Users.Add(user);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}