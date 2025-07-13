using FUMini.BussinessObjects.Models;
using FUMini.Services.Interfaces;
using FUMini.UI.ViewModel.Base;
using System.Collections.ObjectModel;
using System.Windows;

namespace FUMini.UI.ViewModel.User
{
    public class CreateBookingViewModel : BaseViewModel
    {
        #region Services
        private readonly IRoomInformationService _roomInformationService;
        private readonly IRoomTypeService _roomTypeService;
        private readonly IBookingReservationService _bookingReservationService;
        private readonly IBookingDetailService _bookingDetailService;
        private readonly Customer _currentCustomer;
        #endregion

        #region Properties
        // Date
        private DateTime? _checkInDate;
        private DateTime? _checkOutDate;

        // Room Selection
        private RoomType _selectedRoomType;
        private decimal? _maxPrice;
        private ObservableCollection<RoomInformation> _availableRooms;
        private RoomInformation _selectedRoom;
        private ObservableCollection<RoomType> _roomTypes;

        // Summary
        private int _numberOfDays;
        private decimal _totalPrice;
        private string _errorMessage;
        #endregion

        #region Commands
        public RelayCommand SearchRoomsCommand { get; set; }
        public RelayCommand CreateBookingCommand { get; set; }
        public RelayCommand ClearCommand { get; set; }
        #endregion

        #region Constructor
        public CreateBookingViewModel(
            Customer currentCustomer,
            IRoomInformationService roomInformationService,
            IRoomTypeService roomTypeService,
            IBookingReservationService bookingReservationService,
            IBookingDetailService bookingDetailService)
        {
            _currentCustomer = currentCustomer;
            _roomInformationService = roomInformationService;
            _roomTypeService = roomTypeService;
            _bookingReservationService = bookingReservationService;
            _bookingDetailService = bookingDetailService;

            RoomTypes = new ObservableCollection<RoomType>(_roomTypeService.GetAll());
            AvailableRooms = new ObservableCollection<RoomInformation>();

            CheckInDate = DateTime.Today;
            CheckOutDate = DateTime.Today.AddDays(1);

            SearchRoomsCommand = new RelayCommand(ExecuteSearchRooms);
            CreateBookingCommand = new RelayCommand(ExecuteCreateBooking);
            ClearCommand = new RelayCommand(ExecuteClear);
        }
        #endregion

        #region Properties with Notify
        public DateTime? CheckInDate
        {
            get => _checkInDate;
            set
            {
                if (SetProperty(ref _checkInDate, value))
                {
                    UpdateNumberOfDays();
                    UpdateTotalPrice();
                }
            }
        }

        public DateTime? CheckOutDate
        {
            get => _checkOutDate;
            set
            {
                if (SetProperty(ref _checkOutDate, value))
                {
                    UpdateNumberOfDays();
                    UpdateTotalPrice();
                }
            }
        }

        public RoomType SelectedRoomType
        {
            get => _selectedRoomType;
            set => SetProperty(ref _selectedRoomType, value);
        }

        public decimal? MaxPrice
        {
            get => _maxPrice;
            set => SetProperty(ref _maxPrice, value);
        }

        public ObservableCollection<RoomInformation> AvailableRooms
        {
            get => _availableRooms;
            set => SetProperty(ref _availableRooms, value);
        }

        public RoomInformation SelectedRoom
        {
            get => _selectedRoom;
            set
            {
                if (SetProperty(ref _selectedRoom, value))
                {
                    UpdateTotalPrice();
                }
            }
        }

        public ObservableCollection<RoomType> RoomTypes
        {
            get => _roomTypes;
            set => SetProperty(ref _roomTypes, value);
        }

        public int NumberOfDays
        {
            get => _numberOfDays;
            set => SetProperty(ref _numberOfDays, value);
        }

        public decimal TotalPrice
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

        #region Command Methods
        private void ExecuteSearchRooms()
        {
            ErrorMessage = "";

            if (!CheckInDate.HasValue || !CheckOutDate.HasValue)
            {
                ErrorMessage = "Please select check-in and check-out dates!";
                return;
            }

            if (CheckOutDate <= CheckInDate)
            {
                ErrorMessage = "Check-out date must be after check-in date!";
                return;
            }

            var allRooms = _roomInformationService.GetAll();
            var filtered = allRooms.Where(r =>
                (SelectedRoomType == null || r.RoomTypeID == SelectedRoomType.RoomTypeID) &&
                (!MaxPrice.HasValue || r.RoomPricePerDay <= MaxPrice) &&
                r.RoomStatus == 1
            ).ToList();

            AvailableRooms = new ObservableCollection<RoomInformation>(filtered);
        }

        private void ExecuteCreateBooking()
        {
            ErrorMessage = "";

            if (SelectedRoom == null)
            {
                ErrorMessage = "Please select a room!";
                return;
            }

            if (!CheckInDate.HasValue || !CheckOutDate.HasValue)
            {
                ErrorMessage = "Please select check-in and check-out dates!";
                return;
            }

            if (CheckOutDate <= CheckInDate)
            {
                ErrorMessage = "Check-out date must be after check-in date!";
                return;
            }

            try
            {
                var booking = new BookingReservation
                {
                    CustomerID = _currentCustomer.CustomerID,
                    BookingDate = DateOnly.FromDateTime(DateTime.Today),
                    BookingStatus = 1,
                    TotalPrice = TotalPrice
                };
                _bookingReservationService.Add(booking);

                var bookingDetail = new BookingDetail
                {
                    BookingReservationID = booking.BookingReservationID,
                    RoomID = SelectedRoom.RoomID,
                    StartDate = DateOnly.FromDateTime(CheckInDate.Value),
                    EndDate = DateOnly.FromDateTime(CheckOutDate.Value),
                    ActualPrice = SelectedRoom.RoomPricePerDay
                };
                _bookingDetailService.Add(bookingDetail);

                MessageBox.Show("Booking successful!", "Notification",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                ExecuteClear(); // Reset form sau khi đặt phòng thành công
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error booking: {ex.Message}";
            }
        }

        private void ExecuteClear()
        {
            CheckInDate = DateTime.Today;
            CheckOutDate = DateTime.Today.AddDays(1);
            SelectedRoomType = null;
            MaxPrice = null;
            AvailableRooms = new ObservableCollection<RoomInformation>();
            SelectedRoom = null;
            NumberOfDays = 0;
            TotalPrice = 0;
            ErrorMessage = "";
        }
        #endregion

        #region Helper Methods 
        private void UpdateNumberOfDays()
        {
            if (CheckInDate.HasValue && CheckOutDate.HasValue && CheckOutDate > CheckInDate)
                NumberOfDays = (CheckOutDate.Value - CheckInDate.Value).Days;
            else
                NumberOfDays = 0;
        }

        private void UpdateTotalPrice()
        {
            if (SelectedRoom != null && NumberOfDays > 0 && SelectedRoom.RoomPricePerDay.HasValue)
                TotalPrice = NumberOfDays * SelectedRoom.RoomPricePerDay.Value;
            else
                TotalPrice = 0;
        }
        #endregion
    }
}
