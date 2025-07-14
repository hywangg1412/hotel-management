using FUMini.BussinessObjects.Models;
using FUMini.Services.Interfaces;
using FUMini.UI.ViewModel.Base;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace FUMini.UI.ViewModel.Admin
{
    public class BookingManagementViewModel : BaseViewModel
    {
        private readonly IBookingReservationService _bookingReservationService;
        private readonly IBookingDetailService _bookingDetailService;
        private readonly ICustomerService _customerService;
        private readonly IRoomInformationService _roomInformationService;
        private ObservableCollection<BookingReservation> _bookings;
        private ObservableCollection<BookingDetail> _bookingDetails;
        private BookingReservation _selectedBooking;
        private BookingDetail _selectedBookingDetail;
        private bool _isEditing;
        private string _searchText;

        public BookingManagementViewModel(
            IBookingReservationService bookingReservationService,
            IBookingDetailService bookingDetailService,
            ICustomerService customerService,
            IRoomInformationService roomInformationService)
        {
            _bookingReservationService = bookingReservationService;
            _bookingDetailService = bookingDetailService;
            _customerService = customerService;
            _roomInformationService = roomInformationService;
            LoadData();
            InitializeCommands();
        }

        #region Properties
        public ObservableCollection<BookingReservation> Bookings
        {
            get => _bookings;
            set => SetProperty(ref _bookings, value);
        }

        public ObservableCollection<BookingDetail> BookingDetails
        {
            get => _bookingDetails;
            set => SetProperty(ref _bookingDetails, value);
        }

        public BookingReservation SelectedBooking
        {
            get => _selectedBooking;
            set
            {
                SetProperty(ref _selectedBooking, value);
                if (value != null)
                {
                    LoadBookingDetails(value.BookingReservationID);
                }
            }
        }

        public BookingDetail SelectedBookingDetail
        {
            get => _selectedBookingDetail;
            set => SetProperty(ref _selectedBookingDetail, value);
        }

        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                SetProperty(ref _isEditing, value);
                OnPropertyChanged(nameof(IsNotEditing));
            }
        }

        public bool IsNotEditing => !IsEditing;

        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                FilterBookings();
            }
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand { get; private set; }
        public ICommand UpdateBookingStatusCommand { get; private set; }
        public ICommand DeleteBookingCommand { get; private set; }
        public ICommand ViewBookingDetailsCommand { get; private set; }
        #endregion

        #region Methods
        private void InitializeCommands()
        {
            RefreshCommand = new RelayCommand(LoadData);
            UpdateBookingStatusCommand = new RelayCommand(UpdateBookingStatus, CanUpdateBookingStatus);
            DeleteBookingCommand = new RelayCommand(DeleteBooking, CanDeleteBooking);
            ViewBookingDetailsCommand = new RelayCommand(ViewBookingDetails, CanViewBookingDetails);
        }

        private void LoadData()
        {
            var bookings = _bookingReservationService.GetAll();
            Bookings = new ObservableCollection<BookingReservation>(bookings);
        }

        private void LoadBookingDetails(int bookingReservationId)
        {
            var details = _bookingDetailService.GetAll()
                .Where(bd => bd.BookingReservationID == bookingReservationId)
                .ToList();
            BookingDetails = new ObservableCollection<BookingDetail>(details);
        }

        private void FilterBookings()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                LoadData();
                return;
            }

            var filteredBookings = _bookingReservationService.GetAll()
                .Where(b => b.Customer.CustomerFullName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                           b.BookingReservationID.ToString().Contains(SearchText))
                .ToList();

            Bookings = new ObservableCollection<BookingReservation>(filteredBookings);
        }

        private void UpdateBookingStatus()
        {
            if (SelectedBooking == null) return;

            try
            {
                var statusOptions = new[] { "Pending", "Confirmed", "Cancelled", "Completed" };
                var currentStatus = GetBookingStatusText(SelectedBooking.BookingStatus ?? 0);
                
                var result = MessageBox.Show(
                    $"Current status: {currentStatus}\n\nSelect new status:\n" +
                    "0 - Pending\n1 - Confirmed\n2 - Cancelled\n3 - Completed",
                    "Update Booking Status",
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.OK)
                {
                    // In a real application, you'd show a dialog to select status
                    // For now, we'll cycle through statuses
                    var newStatus = (byte)((SelectedBooking.BookingStatus + 1) % 4);
                    SelectedBooking.BookingStatus = newStatus;
                    
                    _bookingReservationService.Update(SelectedBooking);
                    MessageBox.Show($"Booking status updated to: {GetBookingStatusText(newStatus)}", 
                                  "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating booking status: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanUpdateBookingStatus()
        {
            return SelectedBooking != null && !IsEditing;
        }

        private void DeleteBooking()
        {
            if (SelectedBooking == null) return;

            try
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to delete booking #{SelectedBooking.BookingReservationID}?\n" +
                    $"This will also delete all associated booking details.",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Delete booking details first
                    var details = _bookingDetailService.GetAll()
                        .Where(bd => bd.BookingReservationID == SelectedBooking.BookingReservationID)
                        .ToList();

                    foreach (var detail in details)
                    {
                        _bookingDetailService.Delete(detail);
                    }

                    // Delete the booking reservation
                    _bookingReservationService.Delete(SelectedBooking.BookingReservationID);
                    MessageBox.Show("Booking deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting booking: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanDeleteBooking()
        {
            return SelectedBooking != null && !IsEditing;
        }

        private void ViewBookingDetails()
        {
            if (SelectedBooking != null)
            {
                LoadBookingDetails(SelectedBooking.BookingReservationID);
            }
        }

        private bool CanViewBookingDetails()
        {
            return SelectedBooking != null;
        }

        private string GetBookingStatusText(byte status)
        {
            return status switch
            {
                0 => "Pending",
                1 => "Confirmed", 
                2 => "Cancelled",
                3 => "Completed",
                _ => "Unknown"
            };
        }
        #endregion
    }
}
