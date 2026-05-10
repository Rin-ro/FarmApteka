using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using AptekaLib;
using Apteka.Services;

namespace Apteka
{
    public partial class OrdersPageAdd : Window
    {
        private readonly ApiService _api = App.Api;
        public ObservableCollection<OrderItemRequest> Items { get; } = new();

        public OrdersPageAdd() { InitializeComponent(); DataContext = this; }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Items.Count == 0) { MessageBox.Show("Добавьте позиции"); return; }
            var req = new CreateOrderRequest
            {
                UserId = App.CurrentUser?.Id ?? 1,
                Items = Items.ToList()
            };
            var order = await _api.CreateOrderAsync(req);
            if (order != null) DialogResult = true; else MessageBox.Show("Ошибка создания заказа");
        }
        private void RegBtn_Click(object sender, RoutedEventArgs e) => DialogResult = false;
        private void BtnAddItem_Click(object sender, RoutedEventArgs e) { /* открыть выбор лекарства */ }
    }
}