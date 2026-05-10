using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
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

        private async void LoadOrders()
        {
            try
            {
                if (App.CurrentUser != null)
                {
                    var list = await _api.GetUserOrdersAsync(App.CurrentUser.Id);
                    Orders.Clear();
                    foreach (var o in list)
                        Orders.Add(o);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}