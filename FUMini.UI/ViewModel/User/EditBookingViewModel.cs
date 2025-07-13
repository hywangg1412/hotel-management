using FUMini.BussinessObjects.Models;
using FUMini.Services.Interfaces;
using FUMini.UI.ViewModel.Base;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace FUMini.UI.ViewModel.User
{
    public class EditBookingViewModel : BaseViewModel
    {
        #region Private Fields
        private readonly BookingReservation _originalBooking;
        private readonly IBookingReservationService _bookingReservationService;
        private readonly IBookingDetailService _bookingDetailService;
        private readonly IRoomInformationService _roomInformationService;

        private DateTime? _checkInDate;
        private DateTime? _checkOutDate;
        private decimal? _totalPrice;
        private string _errorMessage;
        private RoomInformation _selectedRoom;
        private ObservableCollection<RoomInformation> _availableRooms;
        private int _numberOfDays;
        #endregion

        #region Constructor
        public EditBookingViewModel(
            BookingReservation booking,
            IBookingReservationService bookingReservationService,
            IBookingDetailService bookingDetailService,
            IRoomInformationService roomInformationService)
        {
            _originalBooking = booking;
            _bookingReservationService = bookingReservationService;
            _bookingDetailService = bookingDetailService;
            _roomInformationService = roomInformationService;

            InitializeCommands();
            LoadBookingData();
        }
        #endregion

        #region Properties
        public int BookingReservationID { get; set; }
        
        public DateTime? CheckInDate
        {
            get => _checkInDate;
            set
            {
                SetProperty(ref _checkInDate, value);
                UpdateNumberOfDays();
                UpdateTotalPrice();
                ClearErrorMessage();
            }
        }

        public DateTime? CheckOutDate
        {
            get => _checkOutDate;
            set
            {
                SetProperty(ref _checkOutDate, value);
                UpdateNumberOfDays();
                UpdateTotalPrice();
                ClearErrorMessage();
            }
        }

        public RoomInformation SelectedRoom
        {
            get => _selectedRoom;
            set
            {
                SetProperty(ref _selectedRoom, value);
                UpdateTotalPrice();
                ClearErrorMessage();
            }
        }

        public ObservableCollection<RoomInformation> AvailableRooms
        {
            get => _availableRooms;
            set => SetProperty(ref _availableRooms, value);
        }

        public int NumberOfDays
        {
            get => _numberOfDays;
            set => SetProperty(ref _numberOfDays, value);
        }

        public decimal? TotalPrice
        {
            get => _totalPrice;
            set => SetProperty(ref _totalPrice, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }
        #endregion

        #region Commands
        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand LoadAvailableRoomsCommand { get; private set; }
        #endregion

        #region Events
        public event Action? RequestClose;
        #endregion

        #region Initialization Methods
        private void InitializeCommands()
        {
            SaveCommand = new RelayCommand(ExecuteSave, CanExecuteSave);
            CancelCommand = new RelayCommand(ExecuteCancel);
            LoadAvailableRoomsCommand = new RelayCommand(LoadAvailableRooms);
        }

        private void LoadBookingData()
        {
            BookingReservationID = _originalBooking.BookingReservationID;
            TotalPrice = _originalBooking.TotalPrice;

            // Load booking details for dates and room
            var bookingDetails = _bookingDetailService.GetAll()
                .Where(bd => bd.BookingReservationID == _originalBooking.BookingReservationID)
                .FirstOrDefault();

            if (bookingDetails != null)
            {
                CheckInDate = bookingDetails.StartDate.ToDateTime(TimeOnly.MinValue);
                CheckOutDate = bookingDetails.EndDate.ToDateTime(TimeOnly.MinValue);
                
                SelectedRoom = _roomInformationService.GetByID(bookingDetails.RoomID);
            }

            AvailableRooms = new ObservableCollection<RoomInformation>();
            UpdateNumberOfDays();
            
            LoadAvailableRooms();
        }
        #endregion

        #region Command Methods
        private void ExecuteSave()
        {
            ErrorMessage = "";

            if (!ValidateInput())
                return;

            try
            {
                UpdateBookingDetails();
                UpdateBooking();
                
                ShowSuccessMessage("Booking updated successfully!");
                RequestClose?.Invoke();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error updating booking: {ex.Message}";
            }
        }

        private bool CanExecuteSave()
        {
            return CheckInDate.HasValue && CheckOutDate.HasValue && 
                   CheckOutDate > CheckInDate && SelectedRoom != null;
        }

        private void ExecuteCancel()
        {
            RequestClose?.Invoke();
        }
        #endregion

        #region Helper Methods
        private bool ValidateInput()
        {
            if (!CheckInDate.HasValue || !CheckOutDate.HasValue)
            {
                ErrorMessage = "Please select check-in and check-out dates!";
                return false;
            }

            if (CheckOutDate <= CheckInDate)
            {
                ErrorMessage = "Check-out date must be after check-in date!";
                return false;
            }

            if (SelectedRoom == null)
            {
                ErrorMessage = "Please select a room!";
                return false;
            }

            return true;
        }

        private void UpdateBookingDetails()
        {
            var bookingDetails = _bookingDetailService.GetAll()
                .Where(bd => bd.BookingReservationID == BookingReservationID)
                .FirstOrDefault();

            if (bookingDetails != null)
            {
                if (bookingDetails.RoomID != SelectedRoom.RoomID)
                {
                    var newBookingDetail = new BookingDetail
                    {
                        BookingReservationID = BookingReservationID,
                        RoomID = SelectedRoom.RoomID,
                        StartDate = DateOnly.FromDateTime(CheckInDate.Value),
                        EndDate = DateOnly.FromDateTime(CheckOutDate.Value),
                        ActualPrice = SelectedRoom.RoomPricePerDay
                    };
                    _bookingDetailService.UpdateWithRoomChange(BookingReservationID, bookingDetails.RoomID, newBookingDetail);
                }
                else
                {
                    // Nếu không đổi phòng, chỉ update ngày và giá
                    bookingDetails.StartDate = DateOnly.FromDateTime(CheckInDate.Value);
                    bookingDetails.EndDate = DateOnly.FromDateTime(CheckOutDate.Value);
                    bookingDetails.ActualPrice = SelectedRoom.RoomPricePerDay;
                    _bookingDetailService.Update(bookingDetails);
                }
            }
        }

        private void UpdateBooking()
        {
            _originalBooking.TotalPrice = TotalPrice;
            _bookingReservationService.Update(_originalBooking);
        }

        private void UpdateNumberOfDays()
        {
            if (CheckInDate.HasValue && CheckOutDate.HasValue && CheckOutDate > CheckInDate)
            {
                NumberOfDays = (CheckOutDate.Value - CheckInDate.Value).Days;
            }
            else
            {
                NumberOfDays = 0;
            }
        }

        private void UpdateTotalPrice()
        {
            if (SelectedRoom != null && NumberOfDays > 0)
            {
                TotalPrice = SelectedRoom.RoomPricePerDay * NumberOfDays;
            }
            else
            {
                TotalPrice = 0;
            }
        }

        private void LoadAvailableRooms()
        {
            ErrorMessage = "";

            if (!CheckInDate.HasValue || !CheckOutDate.HasValue)
            {
                LoadAllActiveRooms();
                return;
            }

            if (CheckOutDate <= CheckInDate)
            {
                ErrorMessage = "Check-out date must be after check-in date!";
                return;
            }

            LoadAllActiveRooms();
            ErrorMessage = "";
        }

        private void LoadAllActiveRooms()
        {
            var allRooms = _roomInformationService.GetAll();
            var availableRooms = allRooms.Where(r => r.RoomStatus == 1).ToList();

            if (SelectedRoom != null && !availableRooms.Any(r => r.RoomID == SelectedRoom.RoomID))
            {
                availableRooms.Add(SelectedRoom);
            }

            AvailableRooms = new ObservableCollection<RoomInformation>(availableRooms);
        }

        private void ClearErrorMessage()
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ErrorMessage = "";
            }
        }

        private void ShowSuccessMessage(string message)
        {
            MessageBox.Show(message, "Notification", 
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion
    }
} 