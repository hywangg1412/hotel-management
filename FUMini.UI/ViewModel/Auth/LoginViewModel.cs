using FUMini.BussinessObjects.Models;
using FUMini.Services.Interfaces;
using FUMini.UI.Ultis;
using FUMini.UI.ViewModel.Base;
using System.Windows;
using System.Windows.Input;

namespace FUMini.UI.ViewModel.Auth
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly ICustomerService _customerService;
        private readonly AdminConfig _adminConfig;
        private string _email;
        private string _password;
        private string _errorMessage;
        private bool _isErrorVisible;
        public event Action? RequestClose;

        public LoginViewModel(ICustomerService customerService, AdminConfig adminConfig)
        {
            _customerService = customerService;
            _adminConfig = adminConfig;

            LoginCommand = new RelayCommand(ExecuteLogin, CanExecuteLogin);
            RegisterCommand = new RelayCommand(ExecuteRegister);
            ClearCommand = new RelayCommand(ExecuteClear);
        }

        #region Properties
        public string Email
        {
            get => _email;
            set
            {
                if (SetProperty(ref _email, value))
                {
                    ((RelayCommand)LoginCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (SetProperty(ref _password, value))
                {
                    ((RelayCommand)LoginCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                SetProperty(ref _errorMessage, value);
                IsErrorVisible = !string.IsNullOrEmpty(value);
            }
        }

        public bool IsErrorVisible
        {
            get => _isErrorVisible;
            set => SetProperty(ref _isErrorVisible, value);
        }
        #endregion

        #region Commands
        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }
        public ICommand ClearCommand { get; }
        #endregion

        #region Command Methods
        private void ExecuteLogin()
        {
            ErrorMessage = "";

            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "Please enter your email and password";
                return;
            }

            if (!Validators.IsValidEmail(Email))
            {
                ErrorMessage = "Please enter a valid email address";
                return;
            }

            // Check Admin Account
            if (Email == _adminConfig.Email && Password == _adminConfig.Password)
            {
                MessageBox.Show("Hello Admin!", "Welcome", MessageBoxButton.OK, MessageBoxImage.Information);
                OpenMainWindow(true, null);
                return;
            }

            // Check Customer Account
            var customer = _customerService.FindCustomerByEmail(_email);

            if (customer == null)
            {
                ErrorMessage = "Email does not exist, please sign up";
                return;
            }

            if (customer.Password != Password)
            {
                ErrorMessage = "Invalid password";
                return;
            }
            _customerService.UpdateStatus(customer.CustomerID, 1);

            MessageBox.Show($"Welcome {customer.CustomerFullName}!", "Login Successful", MessageBoxButton.OK, MessageBoxImage.Information);
            OpenMainWindow(false, customer);
        }

        private bool CanExecuteLogin()
        {
            return !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password);
        }

        private void ExecuteRegister()
        {
            var registerWindow = new View.RegisterWindow(_customerService);
            registerWindow.ShowDialog();
        }

        private void ExecuteClear()
        {
            Email = "";
            Password = "";
            ErrorMessage = "";
        }
        #endregion

        #region Helper Methods

        private void OpenMainWindow(bool isAdmin, Customer? customer)
        {
            var vm = new MainWindowViewModel(isAdmin, _customerService, customer);
            var mainWindow = new MainWindow(vm);
            mainWindow.Show();
            CloseCurrentWindow();
        }

        private void CloseCurrentWindow()
        {
            RequestClose?.Invoke();
        }
        #endregion
    }
}
