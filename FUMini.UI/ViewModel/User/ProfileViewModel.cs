using FUMini.BussinessObjects.Models;
using FUMini.UI.ViewModel.Base;
using System.Windows.Input;

namespace FUMini.UI.ViewModel.User
{
    public class ProfileViewModel : BaseViewModel
    {
        public int CustomerID { get; set; }
        public string CustomerFullName { get; set; }
        public string Telephone { get; set; }
        public string EmailAddress { get; set; }
        public DateOnly? CustomerBirthday { get; set; }
        public byte? CustomerStatus { get; set; }
        public ICommand EditProfileCommand { get; set; }

        public ProfileViewModel(Customer customer, ICommand editProfileCommand)
        {
            CustomerID = customer.CustomerID;
            CustomerFullName = customer.CustomerFullName;
            Telephone = customer.Telephone;
            EmailAddress = customer.EmailAddress;
            CustomerStatus = customer.CustomerStatus;
            EditProfileCommand = editProfileCommand;
        }

        public Customer ToCustomer()
        {
            return new Customer
            {
                CustomerID = this.CustomerID,
                CustomerFullName = this.CustomerFullName,
                Telephone = this.Telephone,
                EmailAddress = this.EmailAddress,
                CustomerBirthday = this.CustomerBirthday,
                CustomerStatus = this.CustomerStatus
            };
        }
    }
}
