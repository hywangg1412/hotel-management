using FUMini.UI.View;
using FUMini.UI.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace FUMini.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;

            if (vm.AdminDashboardVM != null)
                vm.AdminDashboardVM.RequestLogout += OnRequestLogout;
            if (vm.UserDashboardVM != null)
                vm.UserDashboardVM.RequestLogout += OnRequestLogout;
        }

        private void OnRequestLogout()
        {
            var serviceProvider = ((App)Application.Current).Services;
            var loginWindow = serviceProvider.GetRequiredService<LoginWindow>();
            Application.Current.MainWindow = loginWindow;
            loginWindow.Show();
            this.Close();
        }
    }
}