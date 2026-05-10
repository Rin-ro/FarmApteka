using Apteka.Services;
using AptekaLib;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Apteka
{
    public partial class OrdersPageAdd : Window
    {
        private readonly ApiService _api = App.Api;
        public ObservableCollection<OrderItemRequest> Items { get; } = new();

        public OrdersPageAdd()
        {
            InitializeComponent();
            DataContext = this;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (App.CurrentUser == null || Items.Count == 0)
            {
                MessageBox.Show("Добавьте товары в заказ", "Ошибка");
                return;
            }

            try
            {
                var request = new CreateOrderRequest
                {
                    UserId = App.CurrentUser.Id,
                    PaymentMethod = "Наличные", // Или из ComboBox
                    DeliveryMethod = "Самовывоз",
                    Items = Items.ToList()
                };

                var order = await _api.CreateOrderAsync(request);
                if (order != null)
                    DialogResult = true;
                else
                    MessageBox.Show("Ошибка при создании заказа", "Ошибка");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void RegBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void BtnAddItem_Click(object sender, RoutedEventArgs e)
        {
            // Открыть окно выбора лекарства
        }
    }
}