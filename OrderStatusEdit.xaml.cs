using Apteka.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Apteka
{
    public partial class OrderStatusEdit : Window
    {
        private readonly ApiService _api = App.Api;
        private readonly int _orderId;

        public OrderStatusEdit(int orderId, string currentStatus)
        {
            InitializeComponent();
            _orderId = orderId;
            // Устанавливаем текущий статус в ComboBox
            foreach (ComboBoxItem item in StatusCombo.Items)
            {
                if (item.Content.ToString() == currentStatus)
                {
                    StatusCombo.SelectedItem = item;
                    break;
                }
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string newStatus = ((ComboBoxItem)StatusCombo.SelectedItem).Content.ToString();
            if (await _api.UpdateOrderStatusAsync(_orderId, newStatus))
                DialogResult = true;
            else
                MessageBox.Show("Не удалось изменить статус", "Ошибка");
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}