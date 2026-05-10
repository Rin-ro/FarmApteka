using Apteka.Services;
using AptekaLib;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

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
                var list = await _api.GetUsersAsync();
                Users.Clear();
                foreach (var u in list) Users.Add(u);
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка: {ex.Message}"); }
        }

        public object? GetSelectedItem() => usersDataTable.SelectedItem;
        public void Refresh() => LoadUsers();
    }
}