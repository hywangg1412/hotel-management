using FUMini.BussinessObjects.Models;
using FUMini.Services.Interfaces;
using FUMini.UI.ViewModel.Base;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Input;
using System;

namespace FUMini.UI.ViewModel.Admin
{
    public class AdminDashboardViewModel : BaseViewModel
    {
        private readonly IServiceProvider _serviceProvider;

        #region Properties
        private object _currentAdminView;
        public object CurrentAdminView
        {
            get => _currentAdminView;
            set { _currentAdminView = value; OnPropertyChanged(); }
        }
        #endregion

        #region VM
        public CustomerManagementViewModel CustomerVM { get; set; }
        public RoomManagementViewModel RoomVM { get; set; }
        public BookingManagementViewModel BookingVM { get; set; }
        public ReportViewModel ReportVM { get; set; }
        #endregion

        #region Commands
        public ICommand ShowCustomerManagementCommand { get; }
        public ICommand ShowRoomManagementCommand { get; }
        public ICommand ShowBookingManagementCommand { get; }
        public ICommand ShowReportCommand { get; }
        public ICommand LogoutCommand { get; }
        #endregion

        public event Action? RequestLogout;

        #region Constructor
        public AdminDashboardViewModel(
            ICustomerService customerService,
            IRoomInformationService roomInformationService,
            IRoomTypeService roomTypeService,
            IBookingReservationService bookingReservationService,
            IBookingDetailService bookingDetailService,
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            CustomerVM = _serviceProvider.GetRequiredService<CustomerManagementViewModel>();
            RoomVM = _serviceProvider.GetRequiredService<RoomManagementViewModel>();
            BookingVM = _serviceProvider.GetRequiredService<BookingManagementViewModel>();
            ReportVM = _serviceProvider.GetRequiredService<ReportViewModel>();

            ShowCustomerManagementCommand = new RelayCommand(() => CurrentAdminView = CustomerVM);
            ShowRoomManagementCommand = new RelayCommand(() => CurrentAdminView = RoomVM);
            ShowBookingManagementCommand = new RelayCommand(() => CurrentAdminView = BookingVM);
            ShowReportCommand = new RelayCommand(() => CurrentAdminView = ReportVM);
            LogoutCommand = new RelayCommand(Logout);

            CurrentAdminView = CustomerVM;
        }
        #endregion

        #region Methods
        private void Logout()
        {
            var result = MessageBox.Show("Are you sure you want to logout?", "Confirm Logout",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                RequestLogout?.Invoke();
            }
        }
        #endregion
    }
}
