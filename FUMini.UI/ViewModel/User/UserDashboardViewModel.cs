using FUMini.BussinessObjects.Models;
using FUMini.Services.Interfaces;
using FUMini.UI.ViewModel.Base;
using System.Windows.Input;

namespace FUMini.UI.ViewModel.User
{
    public class UserDashboardViewModel : BaseViewModel
    {
        private object _currentUserView;

        public object CurrentUserView
        {
            get => _currentUserView;
            set { _currentUserView = value; OnPropertyChanged(); }
        }
        public ICustomerService _customerService { get; set; }
        public ProfileViewModel ProfileVM { get; set; }
        public BookingHistoryViewModel BookingHistoryVM { get; set; }
        public UpdateProfileViewModel UpdateProfileVM { get; set; }


        #region Commands
        public RelayCommand ShowProfileCommand { get; }
        public RelayCommand ShowBookingHistoryCommand { get; }
        public ICommand EditProfileCommand { get; }
        #endregion

        public UserDashboardViewModel(Customer customer, ICustomerService CustomerService)
        {
            _customerService = CustomerService;
            EditProfileCommand = new RelayCommand(OpenEditProfile);

            ProfileVM = new ProfileViewModel(customer, EditProfileCommand);
            BookingHistoryVM = new BookingHistoryViewModel(customer);

            ShowProfileCommand = new RelayCommand(() => CurrentUserView = ProfileVM);
            ShowBookingHistoryCommand = new RelayCommand(() => CurrentUserView = BookingHistoryVM);

            CurrentUserView = ProfileVM;
        }

        private void OpenEditProfile()
        {
            var updateVM = new UpdateProfileViewModel(ProfileVM.ToCustomer(), _customerService);
            updateVM.RequestClose += () =>
            {
                var updatedCustomer = _customerService.GetByID(ProfileVM.CustomerID);
                ProfileVM = new ProfileViewModel(updatedCustomer, EditProfileCommand);
                CurrentUserView = ProfileVM;
                OnPropertyChanged(nameof(ProfileVM));
                OnPropertyChanged(nameof(CurrentUserView));
            };
            CurrentUserView = updateVM;
            OnPropertyChanged(nameof(CurrentUserView));
        }

    }
}
