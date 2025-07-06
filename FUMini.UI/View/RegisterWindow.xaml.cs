using FUMini.BussinessObjects.Models;
using FUMini.Services.Interfaces;
using System.Text.RegularExpressions;
using System.Windows;

namespace FUMini.UI.View
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private readonly ICustomerService _customerService;
        public RegisterWindow(ICustomerService customerService)
        {
            InitializeComponent();
            _customerService = customerService;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string fullname = FullNameTextBox.Text.Trim();
            string email = EmailTextBox.Text.Trim();
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;
            string phone = PhoneTextBox.Text.Trim();

            ErrorTextBlock.Text = "";

            if (string.IsNullOrEmpty(fullname) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword) ||
                string.IsNullOrEmpty(phone))
            {
                ErrorTextBlock.Text = "All fields are required!";
                return;
            }

            if (!IsValidEmail(email))
            {
                ErrorTextBlock.Text = "Please enter a valid email address!";
                return;
            }

            if (password != confirmPassword)
            {
                ErrorTextBlock.Text = "Passwords do not match!";
                return;
            }

            if (_customerService.GetAll().Any(c => c.EmailAddress == email))
            {
                ErrorTextBlock.Text = "Email already exists!";
                return;
            }

            var customer = new Customer
            {
                CustomerFullName = fullname,
                EmailAddress = email,
                Password = password,
                Telephone = phone,
            };

            _customerService.Add(customer);

            MessageBox.Show("Register successful! Please login.");
            this.Close();
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                return regex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }
    }
}
