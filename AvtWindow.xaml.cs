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
using System.Windows.Shapes;

namespace Apteka
{
    /// <summary>
    /// Логика взаимодействия для AvtWindow.xaml
    /// </summary>
    public partial class AvtWindow : Window
    {
        string pass = "123";
        string log = "sa";
        public AvtWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string login = LogTB.Text;
            string password = PassTB.Password;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (login == log && password == pass)
            {

                var editWindow = new MainWindow();
                this.Close();

                editWindow.ShowDialog();
            }
            else MessageBox.Show("Ошибка входа", "Введены неверные данные!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void RegBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
