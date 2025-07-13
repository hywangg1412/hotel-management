using FUMini.BussinessObjects.Models;
using FUMini.Services.Interfaces;
using FUMini.UI.View.Admin;
using FUMini.UI.ViewModel.Base;
using FUMini.UI.ViewModel.User;

namespace FUMini.UI.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }

        public AdminDashboardViewModel AdminDashboardVM { get; set; }
        public UserDashboardViewModel UserDashboardVM { get; set; }

        public MainWindowViewModel(
            bool isAdmin, 
            ICustomerService customerService,
            IRoomInformationService roomInformationService,
            IRoomTypeService roomTypeService,
            IBookingReservationService bookingReservationService,
            IBookingDetailService bookingDetailService,
            Customer? customer = null)
        {
            if (isAdmin)
            {
                AdminDashboardVM = new AdminDashboardViewModel();
                CurrentView = AdminDashboardVM;
            }
            else
            {
                if (customer == null)
                {
                    throw new ArgumentNullException(nameof(customer), "Customer must not be null");
                }
                UserDashboardVM = new UserDashboardViewModel(
                    customer, 
                    customerService,
                    roomInformationService,
                    roomTypeService,
                    bookingReservationService,
                    bookingDetailService);
                CurrentView = UserDashboardVM;
            }
        }
    }
}
