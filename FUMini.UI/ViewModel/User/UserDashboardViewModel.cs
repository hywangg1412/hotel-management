using FUMini.BussinessObjects.Models;
using FUMini.Services.Interfaces;
using FUMini.UI.ViewModel.Base;

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

        #region ViewModel
        public ProfileViewModel ProfileVM { get; set; }
        public CreateBookingViewModel CreateBookingVM { get; set; }
        public BookingHistoryViewModel BookingHistoryVM { get; set; }
        public UpdateProfileViewModel UpdateProfileVM { get; set; }
        #endregion

        #region Commands
        public RelayCommand ShowProfileCommand { get; }
        public RelayCommand ShowBookingHistoryCommand { get; }
        public RelayCommand EditProfileCommand { get; }
        public RelayCommand ShowCreateBookingCommand { get; }
        #endregion

        public UserDashboardViewModel(Customer customer,
            ICustomerService CustomerService,
            IRoomInformationService RoomInformationService,
            IRoomTypeService RoomTypeService,
            IBookingReservationService BookingReservationService,
            IBookingDetailService BookingDetailService)
        {
            _customerService = CustomerService;
            EditProfileCommand = new RelayCommand(OpenEditProfile);
            // Initialize VM
            ProfileVM = new ProfileViewModel(customer, EditProfileCommand);
            BookingHistoryVM = new BookingHistoryViewModel(
                customer, 
                BookingReservationService, 
                BookingDetailService, 
                RoomInformationService);
            CreateBookingVM = new CreateBookingViewModel(customer, RoomInformationService, RoomTypeService, BookingReservationService, BookingDetailService);

            // Initialize Commands 
            ShowProfileCommand = new RelayCommand(() => CurrentUserView = ProfileVM);
            ShowBookingHistoryCommand = new RelayCommand(() => CurrentUserView = BookingHistoryVM);
            ShowCreateBookingCommand = new RelayCommand(() => CurrentUserView = CreateBookingVM);

            // Set Default View
            CurrentUserView = ProfileVM;
        }

        #region VM Methods
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
        #endregion
    }
}
