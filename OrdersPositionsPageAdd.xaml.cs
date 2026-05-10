using System;
using System.Windows;
using AptekaLib;
using Apteka.Services;

namespace Apteka
{
    public partial class OrdersPositionsPageAdd : Window
    {
        private readonly ApiService _api = App.Api;
        public OrderPosition? EditedPosition { get; private set; }

        public OrdersPositionsPageAdd() { InitializeComponent(); }
        public OrdersPositionsPageAdd(OrderPosition pos) : this() { EditedPosition = pos; OrderTB.Text = pos.OrderId.ToString(); MedicineTB.Text = pos.MedicineId.ToString(); QuantityTB.Text = pos.Quantity.ToString(); PriceTB.Text = pos.UnitPrice.ToString(); }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(OrderTB.Text, out var oid) || !int.TryParse(MedicineTB.Text, out var mid) ||
                !int.TryParse(QuantityTB.Text, out var qty) || !decimal.TryParse(PriceTB.Text, out var price))
            { MessageBox.Show("Проверьте поля"); return; }
            var pos = new OrderPosition { OrderId = oid, MedicineId = mid, Quantity = qty, UnitPrice = price };
            if (EditedPosition != null) pos.Id = EditedPosition.Id;
            bool ok = EditedPosition == null ? await _api.AddOrderPositionAsync(pos) : await _api.UpdateOrderPositionAsync(pos);
            if (ok) DialogResult = true; else MessageBox.Show("Ошибка сохранения");
        }
        private void RegBtn_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}