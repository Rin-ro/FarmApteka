using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using AptekaLib;
using Apteka.Services;

namespace Apteka
{
    public partial class OrdersPage : Page
    {
        private readonly ApiService _api = App.Api;
        public ObservableCollection<Order> Orders { get; } = new();
        public OrdersPage()
        {
            InitializeComponent();
            DataContext = this;
            LoadOrders();
        }
        private async void LoadOrders() { try { var list = await _api.GetAllOrdersAsync(); Orders.Clear(); foreach (var o in list) Orders.Add(o); } catch (Exception ex) { MessageBox.Show($"Ошибка: {ex.Message}"); } }
        public object? GetSelectedItem() => ordersDataTable.SelectedItem;
        public void Refresh() => LoadOrders();
    }
}