using FUMini.BussinessObjects.Models;
using FUMini.Services.Interfaces;
using FUMini.UI.ViewModel.Base;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace FUMini.UI.ViewModel.Admin
{
    public class RoomManagementViewModel : BaseViewModel
    {
        private readonly IRoomInformationService _roomInformationService;
        private readonly IRoomTypeService _roomTypeService;
        private readonly IBookingDetailService _bookingDetailService;
        private ObservableCollection<RoomInformation> _rooms;
        private ObservableCollection<RoomType> _roomTypes;
        private RoomInformation _selectedRoom;
        private RoomInformation _editingRoom;
        private bool _isEditing;
        private string _searchText;

        public RoomManagementViewModel(
            IRoomInformationService roomInformationService,
            IRoomTypeService roomTypeService,
            IBookingDetailService bookingDetailService)
        {
            _roomInformationService = roomInformationService;
            _roomTypeService = roomTypeService;
            _bookingDetailService = bookingDetailService;
            LoadData();
            InitializeCommands();
        }

        #region Properties
        public ObservableCollection<RoomInformation> Rooms
        {
            get => _rooms;
            set => SetProperty(ref _rooms, value);
        }

        public ObservableCollection<RoomType> RoomTypes
        {
            get => _roomTypes;
            set => SetProperty(ref _roomTypes, value);
        }

        public RoomInformation SelectedRoom
        {
            get => _selectedRoom;
            set
            {
                SetProperty(ref _selectedRoom, value);
                if (value != null)
                {
                    EditingRoom = new RoomInformation
                    {
                        RoomID = value.RoomID,
                        RoomNumber = value.RoomNumber,
                        RoomDetailDescription = value.RoomDetailDescription,
                        RoomMaxCapacity = value.RoomMaxCapacity,
                        RoomStatus = value.RoomStatus,
                        RoomPricePerDay = value.RoomPricePerDay,
                        RoomTypeID = value.RoomTypeID
                    };
                }
            }
        }

        public RoomInformation EditingRoom
        {
            get => _editingRoom;
            set => SetProperty(ref _editingRoom, value);
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
                FilterRooms();
            }
        }
        #endregion

        #region Commands
        public ICommand AddCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand RefreshCommand { get; private set; }
        #endregion

        #region Methods
        private void InitializeCommands()
        {
            AddCommand = new RelayCommand(AddRoom);
            EditCommand = new RelayCommand(EditRoom, CanEditRoom);
            SaveCommand = new RelayCommand(SaveRoom, CanSaveRoom);
            CancelCommand = new RelayCommand(CancelEdit);
            DeleteCommand = new RelayCommand(DeleteRoom, CanDeleteRoom);
            RefreshCommand = new RelayCommand(LoadData);
        }

        private void LoadData()
        {
            var rooms = _roomInformationService.GetAll();
            Rooms = new ObservableCollection<RoomInformation>(rooms);

            var roomTypes = _roomTypeService.GetAll();
            RoomTypes = new ObservableCollection<RoomType>(roomTypes);
        }

        private void FilterRooms()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                LoadData();
                return;
            }

            var filteredRooms = _roomInformationService.GetAll()
                .Where(r => r.RoomNumber.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                           r.RoomDetailDescription.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                .ToList();

            Rooms = new ObservableCollection<RoomInformation>(filteredRooms);
        }

        private void AddRoom()
        {
            EditingRoom = new RoomInformation
            {
                RoomStatus = 1 // Available by default
            };
            IsEditing = true;
            SelectedRoom = null;
        }

        private void EditRoom()
        {
            if (SelectedRoom != null)
            {
                IsEditing = true;
            }
        }

        private bool CanEditRoom()
        {
            return SelectedRoom != null && !IsEditing;
        }

        private void SaveRoom()
        {
            if (EditingRoom == null) return;

            try
            {
                if (string.IsNullOrWhiteSpace(EditingRoom.RoomNumber) ||
                    EditingRoom.RoomTypeID == 0)
                {
                    MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (EditingRoom.RoomID == 0)
                {
                    // New room
                    if (Rooms.Any(r => r.RoomNumber == EditingRoom.RoomNumber))
                    {
                        MessageBox.Show("Room number already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    _roomInformationService.Add(EditingRoom);
                    MessageBox.Show("Room added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Update existing room
                    var existingRoom = _roomInformationService.GetByID(EditingRoom.RoomID);
                    if (existingRoom != null)
                    {
                        existingRoom.RoomNumber = EditingRoom.RoomNumber;
                        existingRoom.RoomDetailDescription = EditingRoom.RoomDetailDescription;
                        existingRoom.RoomMaxCapacity = EditingRoom.RoomMaxCapacity;
                        existingRoom.RoomStatus = EditingRoom.RoomStatus;
                        existingRoom.RoomPricePerDay = EditingRoom.RoomPricePerDay;
                        existingRoom.RoomTypeID = EditingRoom.RoomTypeID;
                        
                        _roomInformationService.Update(existingRoom);
                        MessageBox.Show("Room updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }

                CancelEdit();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving room: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanSaveRoom()
        {
            return EditingRoom != null && IsEditing;
        }

        private void CancelEdit()
        {
            IsEditing = false;
            EditingRoom = null;
            SelectedRoom = null;
        }

        private void DeleteRoom()
        {
            if (SelectedRoom == null) return;

            try
            {
                // Check if room has any booking details
                var bookingDetails = _bookingDetailService.GetAll()
                    .Where(bd => bd.RoomID == SelectedRoom.RoomID)
                    .ToList();

                if (bookingDetails.Any())
                {
                    // Room has booking history - only change status
                    var result = MessageBox.Show(
                        $"Room '{SelectedRoom.RoomNumber}' has booking history. Do you want to change its status to 'Unavailable' instead of deleting?",
                        "Room Has Bookings",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        var room = _roomInformationService.GetByID(SelectedRoom.RoomID);
                        if (room != null)
                        {
                            room.RoomStatus = 0; // Unavailable
                            _roomInformationService.Update(room);
                            MessageBox.Show("Room status changed to 'Unavailable' successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadData();
                        }
                    }
                }
                else
                {
                    // No booking history - safe to delete
                    var result = MessageBox.Show(
                        $"Are you sure you want to delete room '{SelectedRoom.RoomNumber}'?",
                        "Confirm Delete",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        _roomInformationService.Delete(SelectedRoom.RoomID);
                        MessageBox.Show("Room deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting room: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanDeleteRoom()
        {
            return SelectedRoom != null && !IsEditing;
        }
        #endregion
    }
}
