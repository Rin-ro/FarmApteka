using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using AptekaLib;
using Apteka.Services;

namespace Apteka
{
    public partial class CategoriesPage : Page
    {
        private readonly ApiService _api = App.Api;
        public ObservableCollection<Category> Categories { get; } = new();

        public CategoriesPage()
        {
            InitializeComponent();
            DataContext = this;
            LoadCategories();
        }

        private async void LoadCategories()
        {
            try
            {
                var list = await _api.GetCategoriesAsync();
                Categories.Clear();
                foreach (var c in list)
                    Categories.Add(c);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}