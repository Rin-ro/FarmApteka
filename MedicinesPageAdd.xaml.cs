using Apteka.Services;
using AptekaLib;
using System;
using System.Windows;

namespace Apteka
{
    public partial class MedicinesPageAdd : Window
    {
        private readonly ApiService _api = App.Api;
        public Medicine? EditedMedicine { get; private set; }

        public MedicinesPageAdd()
        {
            InitializeComponent();
            LoadCategories();
        }

        public MedicinesPageAdd(Medicine medicine) : this()
        {
            EditedMedicine = medicine;
            NameTB.Text = medicine.Name;
            DescTB.Text = medicine.Description;
            ManufacturerTB.Text = medicine.Fabricator;
            PriceTB.Text = medicine.Price.ToString();
            ExpirationTB.Text = medicine.ExpirationDate.ToString();
            FormTB.Text = medicine.FormOfRelease;
            StockTB.Text = medicine.TheRestOfTheLayout.ToString();
            CategoryTB.Text = medicine.CategoryId.ToString();
            PrescriptionTB.IsChecked = medicine.Prescription;
        }

        private async void LoadCategories()
        {
            var categories = await _api.GetCategoriesAsync();
            // Можно заполнить ComboBox
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTB.Text) || !decimal.TryParse(PriceTB.Text, out var price))
            {
                MessageBox.Show("Заполните название и цену", "Ошибка");
                return;
            }

            try
            {
                var medicine = new Medicine
                {
                    Id = EditedMedicine?.Id ?? 0,
                    Name = NameTB.Text.Trim(),
                    Description = DescTB.Text.Trim(),
                    Fabricator = ManufacturerTB.Text.Trim(),
                    Price = price,
                    ExpirationDate = DateTime.TryParse(ExpirationTB.Text, out var expDate) ? expDate : DateTime.Now.AddYears(1),
                    FormOfRelease = FormTB.Text.Trim(),
                    CategoryId = int.TryParse(CategoryTB.Text, out var cat) ? cat : 1,
                    TheRestOfTheLayout = int.TryParse(StockTB.Text, out var stock) ? stock : 0,
                    Prescription = PrescriptionTB.IsChecked ?? false
                };

                bool success = EditedMedicine == null
                    ? await _api.AddMedicineAsync(medicine)
                    : await _api.UpdateMedicineAsync(medicine);

                if (success)
                    DialogResult = true;
                else
                    MessageBox.Show("Ошибка при сохранении", "Ошибка");
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
    }
}