using System;
using System.Windows;
using AptekaLib;
using Apteka.Services;

namespace Apteka
{
    public partial class MedicinesPageAdd : Window
    {
        private readonly ApiService _api = App.Api;
        public Medicine? EditedMedicine { get; private set; }

        public MedicinesPageAdd() { InitializeComponent(); LoadCategories(); }
        public MedicinesPageAdd(Medicine med) : this() { EditedMedicine = med; NameTB.Text = med.Name; DescTB.Text = med.Description; ManufacturerTB.Text = med.Fabricator; PriceTB.Text = med.Price.ToString(); ExpirationTB.Text = med.ExpirationDate?.ToString("yyyy-MM-dd"); FormTB.Text = med.FormOfRelease; CategoryTB.Text = med.CategoryId.ToString(); StockTB.Text = med.TheRestOfTheLayout.ToString(); PrescriptionTB.IsChecked = med.Prescription; }

        private async void LoadCategories() { /* можно загрузить категории для выбора */ }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(PriceTB.Text, out var price)) { MessageBox.Show("Некорректная цена"); return; }
            int catId = int.TryParse(CategoryTB.Text, out var cid) ? cid : 1;
            int stock = int.TryParse(StockTB.Text, out var st) ? st : 0;
            var med = new Medicine
            {
                Name = NameTB.Text.Trim(),
                Description = DescTB.Text.Trim(),
                Fabricator = ManufacturerTB.Text.Trim(),
                Price = price,
                ExpirationDate = DateTime.TryParse(ExpirationTB.Text, out var exp) ? exp : null,
                FormOfRelease = FormTB.Text.Trim(),
                CategoryId = catId,
                TheRestOfTheLayout = stock,
                Prescription = PrescriptionTB.IsChecked == true
            };
            if (EditedMedicine != null) med.Id = EditedMedicine.Id;
            bool ok = EditedMedicine == null ? await _api.AddMedicineAsync(med) : await _api.UpdateMedicineAsync(med);
            if (ok) DialogResult = true; else MessageBox.Show("Ошибка сохранения");
        }
        private void RegBtn_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}