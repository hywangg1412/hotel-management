using FUMini.BussinessObjects.Models;
using FUMini.Services.Interfaces;
using FUMini.UI.ViewModel.Base;
using System.Windows;
using System.Windows.Input;

namespace FUMini.UI.ViewModel.User
{
    public class UpdateProfileViewModel : BaseViewModel
    {
        public int CustomerID { get; set; }
        public string Fullname { get; set; }
        public string Telephone { get; set; }
        public string EmailAddress { get; set; }
        private DateOnly? _birthday;
        public DateOnly? Birthday 
        { 
            get => _birthday;
            set
            {
                _birthday = value;
                OnPropertyChanged(nameof(Birthday));
                OnPropertyChanged(nameof(BirthdayDateTime));
            }
        }
        public DateTime? BirthdayDateTime
        {
            get => Birthday.HasValue ? Birthday.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null;
            set
            {
                Birthday = value.HasValue ? DateOnly.FromDateTime(value.Value) : (DateOnly?)null;
                OnPropertyChanged(nameof(BirthdayDateTime));
            }
        }
        public byte? Status { get; set; }

        private readonly ICustomerService _customerService;

        public ICommand SaveCommand { get; set; }
        public event Action? RequestClose;

        public UpdateProfileViewModel(Customer customer, ICustomerService customerService)
        {
            CustomerID = customer.CustomerID;
            Fullname = customer.CustomerFullName;
            Telephone = customer.Telephone;
            EmailAddress = customer.EmailAddress;
            Birthday = customer.CustomerBirthday;
            Status = customer.CustomerStatus;
            _customerService = customerService;
            SaveCommand = new RelayCommand(ExecuteSave);

            OnPropertyChanged(nameof(BirthdayDateTime));
        }

        #region Command Methods
        private void ExecuteSave()
        {
            var oldCustomer = _customerService.GetByID(this.CustomerID);

            var updatedCustomer = new Customer
            {
                CustomerID = this.CustomerID,
                CustomerFullName = this.Fullname,
                Telephone = this.Telephone,
                EmailAddress = this.EmailAddress,
                CustomerBirthday = this.Birthday,
                CustomerStatus = this.Status,
                Password = oldCustomer?.Password
            };
            try
            {
                _customerService.Update(updatedCustomer);
                MessageBox.Show("Update profile successfully!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                RequestClose?.Invoke();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                if (ex.InnerException != null)
                    message += "\n" + ex.InnerException.Message;
                if (ex.InnerException?.InnerException != null)
                    message += "\n" + ex.InnerException.InnerException.Message;

                MessageBox.Show("Update profile failed!\n" + message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Helper
        public Customer ToCustomer()
        {
            return new Customer
            {
                CustomerID = this.CustomerID,
                CustomerFullName = this.Fullname,
                Telephone = this.Telephone,
                EmailAddress = this.EmailAddress,
                CustomerBirthday = this.Birthday,
                CustomerStatus = this.Status
            };
            #endregion
        }
    }
}
