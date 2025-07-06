using FUMini.DataAccess.Context;
using FUMini.DataAccess.Repositories.Implementation;
using FUMini.DataAccess.Repositories.Interfaces;
using FUMini.Services.Implementations;
using FUMini.Services.Interfaces;
using System.Text.RegularExpressions;
using System.Windows;

namespace FUMini.UI.View
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly ICustomerService _customerService;

        public LoginWindow()
        {
            InitializeComponent();
            var context = new FUMiniContext();
            ICustomerRepository customerRepository = new CustomerRepository(context);
            _customerService = new CustomerService(customerRepository);
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text.Trim();
            string password = PasswordBox.Password;

            ErrorTextBlock.Text = "";

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ErrorTextBlock.Text = "Please enter your email and password";
                return;
            }

            if (!IsValidEmail(email))
            {
                ErrorTextBlock.Text = "Please enter a valid email address";
                return;
            }

            var customer = _customerService.GetAll().FirstOrDefault(c => c.EmailAddress == email);

            if (customer == null)
            {
                ErrorTextBlock.Text = "Email does not exist, please sign up";
                return;
            }

            if (customer.Password != password)
            {
                ErrorTextBlock.Text = "Invalid password";
                return;
            }

            MessageBox.Show($"Welcome {customer.CustomerFullName}!");

            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void RegisterTextBlock_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var registerWindow = new RegisterWindow(_customerService);
            registerWindow.ShowDialog();
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
