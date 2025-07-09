using FUMini.BussinessObjects.Models;
using FUMini.Services.Interfaces;
using FUMini.UI.Ultis;
using FUMini.UI.ViewModel.Base;
using System.Windows;
using System.Windows.Input;

namespace FUMini.UI.ViewModel.Auth
{
    public class RegisterViewModel : BaseViewModel
    {
        private string _fullName;
        private string _email;
        private string _confirmPassword;
        private string _password;
        private string _phone;
        private string _errorMessage;
        public event Action? RequestClose;
        private readonly ICustomerService _customerService;

        #region Properties
        public string FullName
        {
            get => _fullName;
            set
            {
                if (SetProperty(ref _fullName, value))
                {
                    ((RelayCommand)RegisterCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (SetProperty(ref _email, value))
                {
                    ((RelayCommand)RegisterCommand).RaiseCanExecuteChanged();
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
                    ((RelayCommand)RegisterCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                if (SetProperty(ref _confirmPassword, value))
                {
                    ((RelayCommand)RegisterCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                if (SetProperty(ref _phone, value))
                {
                    ((RelayCommand)RegisterCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }
        #endregion

        #region Commands
        public ICommand RegisterCommand { get; }
        public ICommand ClearCommand { get; }
        #endregion


        #region Command Methods
        public RegisterViewModel(ICustomerService customerService)
        {
            _customerService = customerService;
            RegisterCommand = new RelayCommand(ExecuteRegister, CanExecuteRegister);
            ClearCommand = new RelayCommand(ExecuteClear);
        }

        private void ExecuteRegister()
        {
            ErrorMessage = "";

            if (string.IsNullOrWhiteSpace(FullName) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(ConfirmPassword) ||
                string.IsNullOrWhiteSpace(Phone))
            {
                ErrorMessage = "All fields are required!";
                return;
            }

            if (!Validators.IsValidEmail(Email))
            {
                ErrorMessage = "Please enter a valid email address";
                return;
            }

            if (_customerService.IsEmailExist(Email))
            {
                ErrorMessage = "Email already exists, try another one";
                return;
            }

            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Passwords do not match, please check again";
                return;
            }

            if (!Validators.IsValidPhone(Phone))
            {
                ErrorMessage = "Phone number must be 10 or 11 digits";
                return;
            }

            var customer = new Customer
            {
                CustomerFullName = FullName,
                EmailAddress = Email,
                Password = Password,
                Telephone = Phone
            };

            _customerService.Add(customer);

            MessageBox.Show($"Welcome {customer.CustomerFullName}!", "Register Successful, Redirect to login window", MessageBoxButton.OK, MessageBoxImage.Information);
            CloseCurrentWindow();
        }

        #endregion

        #region Helper Method
        private bool CanExecuteRegister()
        {
            return !string.IsNullOrWhiteSpace(FullName)
                && !string.IsNullOrWhiteSpace(Email)
                && !string.IsNullOrWhiteSpace(Password)
                && !string.IsNullOrWhiteSpace(ConfirmPassword)
                && !string.IsNullOrWhiteSpace(Phone);
        }

        private void ExecuteClear()
        {
            FullName = "";
            Email = "";
            Password = "";
            ConfirmPassword = "";
            Phone = "";
            ErrorMessage = "";
        }

        private void CloseCurrentWindow()
        {
            RequestClose?.Invoke();
        }
        #endregion
    }
}

