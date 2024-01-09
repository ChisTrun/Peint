using Custom_Paint.Navigation;
using Custom_Paint.ViewModels;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Custom_Paint
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private AppNavigation navigation = new AppNavigation();
        protected override void OnStartup(StartupEventArgs e)
        {
            navigation.CurrentVM = new PaintViewModel();
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(navigation)
            };
            MainWindow.Show();
        }
    }

}
