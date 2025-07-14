using FUMini.Services.Interfaces;
using System.Windows;
using FUMini.UI.ViewModel.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace FUMini.UI.View
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow(ICustomerService customerService, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            var viewModel = serviceProvider.GetRequiredService<RegisterViewModel>();
            DataContext = viewModel;
            
            PasswordBox.PasswordChanged += (s, e) =>
            {
                viewModel.Password = PasswordBox.Password;
            };
            
            ConfirmPasswordBox.PasswordChanged += (s, e) =>
            {
                viewModel.ConfirmPassword = ConfirmPasswordBox.Password;
            };
            
            viewModel.RequestClose += () => this.Close();
        }
    }
}
