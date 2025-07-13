// FUMini.UI/ViewModel/User/BookingHistoryViewModel.cs
using FUMini.BussinessObjects.Models;
using FUMini.Services.Interfaces;
using FUMini.UI.ViewModel.Base;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace FUMini.UI.ViewModel.User
{
    public class BookingHistoryViewModel : BaseViewModel
    {
        #region Private Fields
        private readonly Customer _currentCustomer;
        private readonly IBookingReservationService _bookingReservationService;
        private readonly IBookingDetailService _bookingDetailService;
        private readonly IRoomInformationService _roomInformationService;

        private ObservableCollection<BookingReservation> _bookingHistory;
        private BookingReservation _selectedBooking;
        private string _searchText;
        private DateTime? _fromDate;
        private DateTime? _toDate;
        private object _currentUserView;
        public object CurrentUserView
        {
            get => _currentUserView;
            set { _currentUserView = value; OnPropertyChanged(); }
        }
        #endregion

        #region Constructor
        public BookingHistoryViewModel(
            Customer customer,
            IBookingReservationService bookingReservationService,
            IBookingDetailService bookingDetailService,
            IRoomInformationService roomInformationService)
        {
            _currentCustomer = customer;
            _bookingReservationService = bookingReservationService;
            _bookingDetailService = bookingDetailService;
            _roomInformationService = roomInformationService;

            InitializeCommands();
            InitializeData();
            LoadBookingHistory();
        }
        #endregion

        #region Properties
        public ObservableCollection<BookingReservation> BookingHistory
        {
            get => _bookingHistory;
            set => SetProperty(ref _bookingHistory, value);
        }

        public BookingReservation SelectedBooking
        {
            get => _selectedBooking;
            set => SetProperty(ref _selectedBooking, value);
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                LoadBookingHistory();
            }
        }

        public DateTime? FromDate
        {
            get => _fromDate;
            set
            {
                SetProperty(ref _fromDate, value);
                LoadBookingHistory();
            }
        }

        public DateTime? ToDate
        {
            get => _toDate;
            set
            {
                SetProperty(ref _toDate, value);
                LoadBookingHistory();
            }
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand { get; private set; }
        public ICommand EditBookingCommand { get; private set; }
        public ICommand DeleteBookingCommand { get; private set; }
        public ICommand ViewDetailsCommand { get; private set; }
        #endregion

        #region Initialization Methods
        private void InitializeCommands()
        {
            RefreshCommand = new RelayCommand(LoadBookingHistory);
            EditBookingCommand = new RelayCommand(EditBooking, CanEditBooking);
            DeleteBookingCommand = new RelayCommand(DeleteBooking, CanDeleteBooking);
            ViewDetailsCommand = new RelayCommand(ViewDetails, CanViewDetails);
        }

        private void InitializeData()
        {
            BookingHistory = new ObservableCollection<BookingReservation>();
        }
        #endregion

        #region Data Loading Methods
        private void LoadBookingHistory()
        {
            var allBookings = _bookingReservationService.GetAll()
                .Where(b => b.CustomerID == _currentCustomer.CustomerID)
                .ToList();

            var filteredBookings = ApplyFilters(allBookings);
            BookingHistory = new ObservableCollection<BookingReservation>(filteredBookings);
        }

        private List<BookingReservation> ApplyFilters(List<BookingReservation> bookings)
        {
            var filtered = bookings;

            if (!string.IsNullOrEmpty(SearchText))
            {
                filtered = filtered.Where(b =>
                    b.BookingReservationID.ToString().Contains(SearchText) ||
                    b.TotalPrice.ToString().Contains(SearchText)).ToList();
            }

            if (FromDate.HasValue)
            {
                filtered = filtered.Where(b =>
                    b.BookingDate >= DateOnly.FromDateTime(FromDate.Value)).ToList();
            }

            if (ToDate.HasValue)
            {
                filtered = filtered.Where(b =>
                    b.BookingDate <= DateOnly.FromDateTime(ToDate.Value)).ToList();
            }

            return filtered;
        }
        #endregion

        #region Command Methods
        private void EditBooking()
        {
            if (SelectedBooking == null) return;

            var editViewModel = new EditBookingViewModel(
                SelectedBooking,
                _bookingReservationService,
                _bookingDetailService,
                _roomInformationService);

            editViewModel.RequestClose += () =>
            {
                LoadBookingHistory();
                CurrentUserView = this;
                OnPropertyChanged(nameof(CurrentUserView));
            };
            CurrentUserView = editViewModel;
            OnPropertyChanged(nameof(CurrentUserView));
        }

        private bool CanEditBooking()
        {
            return SelectedBooking != null && SelectedBooking.BookingStatus == 1; // Only active bookings
        }

        private void DeleteBooking()
        {
            if (SelectedBooking == null) return;

            var result = ShowDeleteConfirmation();
            if (result == MessageBoxResult.Yes)
            {
                ExecuteDeleteBooking();
            }
        }

        private bool CanDeleteBooking()
        {
            return SelectedBooking != null && SelectedBooking.BookingStatus == 1; // Only active bookings
        }

        private void ViewDetails()
        {
            if (SelectedBooking == null) return;

            var detailsViewModel = new BookingDetailsViewModel(
                SelectedBooking,
                _bookingDetailService,
                _roomInformationService);
            CurrentUserView = detailsViewModel;
        }

        private bool CanViewDetails()
        {
            return SelectedBooking != null;
        }
        #endregion

        #region Helper Methods
        private MessageBoxResult ShowDeleteConfirmation()
        {
            return MessageBox.Show(
                $"Are you sure you want to cancel booking #{SelectedBooking.BookingReservationID}?",
                "Confirm Cancel Booking",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
        }

        private void ExecuteDeleteBooking()
        {
            try
            {
                // Update booking status to cancelled (status = 0)
                SelectedBooking.BookingStatus = 0;
                _bookingReservationService.Update(SelectedBooking);

                ShowSuccessMessage("Booking cancelled successfully!");
                LoadBookingHistory();
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error cancelling booking: {ex.Message}");
            }
        }

        private void ShowSuccessMessage(string message)
        {
            MessageBox.Show(message, "Notification",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
        #endregion
    }
}