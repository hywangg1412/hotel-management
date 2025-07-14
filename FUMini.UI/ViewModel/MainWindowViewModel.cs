using FUMini.BussinessObjects.Models;
using FUMini.Services.Interfaces;
using FUMini.UI.ViewModel.Admin;
using FUMini.UI.ViewModel.Base;
using FUMini.UI.ViewModel.User;
using System;

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
            IServiceProvider serviceProvider,
            Customer? customer = null)
        {
            if (isAdmin)
            {
                AdminDashboardVM = new AdminDashboardViewModel(
                    customerService,
                    roomInformationService,
                    roomTypeService,
                    bookingReservationService,
                    bookingDetailService,
                    serviceProvider);
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
                    bookingDetailService,
                    serviceProvider);
                CurrentView = UserDashboardVM;
            }
        }
    }
}
