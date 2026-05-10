using System.Windows;
using AptekaLib;
using Apteka.Services;

namespace Apteka
{
    public partial class CategoriesPageAdd : Window
    {
        private readonly ApiService _api = App.Api;
        public Category? EditedCategory { get; private set; }

        public CategoriesPageAdd()
        {
            InitializeComponent();
        }

        public CategoriesPageAdd(Category category) : this()
        {
            EditedCategory = category;
            NameTB.Text = category.Name;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTB.Text))
            {
                MessageBox.Show("Введите название категории", "Ошибка");
                return;
            }

            try
            {
                bool success;

                if (EditedCategory == null)
                {
                    // Создание новой категории
                    var newCategory = new Category
                    {
                        Name = NameTB.Text.Trim()
                    };
                    success = await _api.AddCategoryAsync(newCategory);
                }
                else
                {
                    // Обновление существующей
                    EditedCategory.Name = NameTB.Text.Trim();
                    success = await _api.UpdateCategoryAsync(EditedCategory);
                }

                if (success)
                    DialogResult = true;
                else
                    MessageBox.Show("Ошибка при сохранении категории", "Ошибка");
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