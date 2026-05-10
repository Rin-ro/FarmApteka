using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using AptekaLib;
using Apteka.Services;

namespace Apteka
{
    public partial class MedicinesPage : Page
    {
        private readonly ApiService _api = App.Api;
        public ObservableCollection<Medicine> Medicines { get; } = new();

        public MedicinesPage()
        {
            InitializeComponent();
            DataContext = this;
            LoadMedicines();
        }

        private async void LoadMedicines()
        {
            try
            {
                var list = await _api.GetMedicinesAsync();
                Medicines.Clear();
                foreach (var m in list)
                    Medicines.Add(m);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}