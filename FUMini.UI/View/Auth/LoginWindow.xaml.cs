using FUMini.BussinessObjects.Models;
using FUMini.Services.Interfaces;
using FUMini.UI.ViewModel.Auth;
using System.Windows;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace FUMini.UI.View
{
    public partial class LoginWindow : Window
    {
        public LoginWindow(
            ICustomerService customerService,
            AdminConfig adminConfig,
            IServiceProvider serviceProvider)
        {
            InitializeComponent();
            DataContext = serviceProvider.GetRequiredService<LoginViewModel>();
            PasswordBox.PasswordChanged += (s, e) =>
            {
                var viewModel = DataContext as LoginViewModel;
                if (viewModel != null)
                {
                    viewModel.Password = PasswordBox.Password;
                }
            };
            var viewModel = DataContext as LoginViewModel;
            if (viewModel != null)
            {
                viewModel.RequestClose += () => this.Close();
            }
        }
    }
}
