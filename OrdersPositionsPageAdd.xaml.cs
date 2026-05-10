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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Apteka
{
    /// <summary>
    /// Логика взаимодействия для OrdersPositionsPage.xaml
    /// </summary>
    public partial class OrdersPositionsPageAdd : Window
    {
        private readonly ApiService _api = App.Api;
        public OrderPosition? EditedPosition { get; private set; }

        public OrdersPositionsPageAdd()
        {
            InitializeComponent();
        }

        public OrdersPositionsPageAdd(OrderPosition position) : this()
        {
            EditedPosition = position;
            OrderTB.Text = position.OrderId.ToString();
            MedicineTB.Text = position.MedicineId.ToString();
            QuantityTB.Text = position.Quantity.ToString();
            PriceTB.Text = position.UnitPrice.ToString();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(OrderTB.Text, out var orderId) ||
                !int.TryParse(MedicineTB.Text, out var medId) ||
                !int.TryParse(QuantityTB.Text, out var qty) ||
                !decimal.TryParse(PriceTB.Text, out var price))
            {
                MessageBox.Show("Проверьте числовые поля"); return;
            }

            var pos = new OrderPosition
            {
                OrderId = orderId,
                MedicineId = medId,
                Quantity = qty,
                UnitPrice = price
            };
            if (EditedPosition != null) pos.Id = EditedPosition.Id;

            bool success = EditedPosition == null
                ? await _api.AddOrderPositionAsync(pos)
                : await _api.UpdateOrderPositionAsync(pos);

            if (success) DialogResult = true;
            else MessageBox.Show("Ошибка сохранения");
        }

        private void RegBtn_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
