using System.Windows;
using Apteka.Services;

namespace Apteka
{
    public partial class App : Application
    {
        public static ApiService Api { get; } = new ApiService();
        public static AptekaLib.User? CurrentUser { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // Для теста сразу показываем главное окно
            new MainWindow().Show();
        }
    }
}