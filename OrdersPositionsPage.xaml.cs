using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using AptekaLib;
using Apteka.Services;

namespace Apteka
{
    public partial class OrdersPositionsPage : Page
    {
        private readonly ApiService _api = App.Api;
        public ObservableCollection<OrderPosition> Positions { get; } = new();
        public OrdersPositionsPage()
        {
            InitializeComponent();
            DataContext = this;
            LoadPositions();
        }
        private async void LoadPositions() { try { var list = await _api.GetOrderPositionsAsync(); Positions.Clear(); foreach (var p in list) Positions.Add(p); } catch (Exception ex) { MessageBox.Show($"Ошибка: {ex.Message}"); } }
        public object? GetSelectedItem() => ordersDataTable.SelectedItem;
        public void Refresh() => LoadPositions();
    }
}