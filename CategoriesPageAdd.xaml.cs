using System;
using System.Windows;
using AptekaLib;
using Apteka.Services;

namespace Apteka
{
    public partial class CategoriesPageAdd : Window
    {
        private readonly ApiService _api = App.Api;
        public Category? EditedCategory { get; private set; }

        public CategoriesPageAdd() { InitializeComponent(); }
        public CategoriesPageAdd(Category cat) : this() { EditedCategory = cat; NameTB.Text = cat.Name; }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var name = NameTB.Text.Trim();
            if (string.IsNullOrWhiteSpace(name)) { MessageBox.Show("Введите название"); return; }
            var cat = new Category { Name = name };
            if (EditedCategory != null) cat.Id = EditedCategory.Id;
            bool ok = EditedCategory == null ? await _api.AddCategoryAsync(cat) : await _api.UpdateCategoryAsync(cat);
            if (ok) DialogResult = true; else MessageBox.Show("Ошибка сохранения");
        }
        private void RegBtn_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}