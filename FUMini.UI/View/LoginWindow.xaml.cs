using FUMini.BussinessObjects.Models;
using FUMini.Services.Interfaces;
using FUMini.UI.ViewModel.Auth;
using System.Windows;

namespace FUMini.UI.View
{
    public partial class LoginWindow : Window
    {
        public LoginWindow(ICustomerService customerService, AdminConfig adminConfig)
        {
            InitializeComponent();
            var viewModel = new LoginViewModel(customerService, adminConfig);
            DataContext = viewModel;
            PasswordBox.PasswordChanged += (s, e) =>
            {
                viewModel.Password = PasswordBox.Password;
            };
            viewModel.RequestClose += () => this.Close();
        }
    }
}
