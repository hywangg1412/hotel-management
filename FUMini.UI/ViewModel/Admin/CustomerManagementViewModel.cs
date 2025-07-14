using FUMini.BussinessObjects.Models;
using FUMini.Services.Interfaces;
using FUMini.UI.ViewModel.Base;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace FUMini.UI.ViewModel.Admin
{
    public class CustomerManagementViewModel : BaseViewModel
    {
        private readonly ICustomerService _customerService;
        private ObservableCollection<Customer> _customers;
        private Customer _selectedCustomer;
        private Customer _editingCustomer;
        private bool _isEditing;
        private string _searchText;

        public CustomerManagementViewModel(ICustomerService customerService)
        {
            _customerService = customerService;
            LoadCustomers();
            InitializeCommands();
        }

        #region Properties
        public ObservableCollection<Customer> Customers
        {
            get => _customers;
            set => SetProperty(ref _customers, value);
        }

        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                SetProperty(ref _selectedCustomer, value);
                if (value != null)
                {
                    EditingCustomer = new Customer
                    {
                        CustomerID = value.CustomerID,
                        CustomerFullName = value.CustomerFullName,
                        EmailAddress = value.EmailAddress,
                        Telephone = value.Telephone,
                        CustomerBirthday = value.CustomerBirthday,
                        CustomerStatus = value.CustomerStatus,
                        Password = value.Password
                    };
                }
            }
        }

        public Customer EditingCustomer
        {
            get => _editingCustomer;
            set => SetProperty(ref _editingCustomer, value);
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
                FilterCustomers();
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
            AddCommand = new RelayCommand(AddCustomer);
            EditCommand = new RelayCommand(EditCustomer, CanEditCustomer);
            SaveCommand = new RelayCommand(SaveCustomer, CanSaveCustomer);
            CancelCommand = new RelayCommand(CancelEdit);
            DeleteCommand = new RelayCommand(DeleteCustomer, CanDeleteCustomer);
            RefreshCommand = new RelayCommand(LoadCustomers);
        }

        private void LoadCustomers()
        {
            var customers = _customerService.GetAll();
            Customers = new ObservableCollection<Customer>(customers);
        }

        private void FilterCustomers()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                LoadCustomers();
                return;
            }

            var filteredCustomers = _customerService.GetAll()
                .Where(c => c.CustomerFullName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                           c.EmailAddress.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                           c.Telephone.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                .ToList();

            Customers = new ObservableCollection<Customer>(filteredCustomers);
        }

        private void AddCustomer()
        {
            EditingCustomer = new Customer
            {
                CustomerStatus = 1 // Active by default
            };
            IsEditing = true;
            SelectedCustomer = null;
        }

        private void EditCustomer()
        {
            if (SelectedCustomer != null)
            {
                IsEditing = true;
            }
        }

        private bool CanEditCustomer()
        {
            return SelectedCustomer != null && !IsEditing;
        }

        private void SaveCustomer()
        {
            if (EditingCustomer == null) return;

            try
            {
                if (string.IsNullOrWhiteSpace(EditingCustomer.CustomerFullName) ||
                    string.IsNullOrWhiteSpace(EditingCustomer.EmailAddress) ||
                    string.IsNullOrWhiteSpace(EditingCustomer.Telephone))
                {
                    MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (EditingCustomer.CustomerID == 0)
                {
                    // New customer - password is required
                    if (string.IsNullOrWhiteSpace(EditingCustomer.Password))
                    {
                        MessageBox.Show("Password is required for new customers.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    if (_customerService.IsEmailExist(EditingCustomer.EmailAddress))
                    {
                        MessageBox.Show("Email already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    _customerService.Add(EditingCustomer);
                    MessageBox.Show("Customer added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Update existing customer
                    var existingCustomer = _customerService.GetByID(EditingCustomer.CustomerID);
                    if (existingCustomer != null)
                    {
                        existingCustomer.CustomerFullName = EditingCustomer.CustomerFullName;
                        existingCustomer.EmailAddress = EditingCustomer.EmailAddress;
                        existingCustomer.Telephone = EditingCustomer.Telephone;
                        existingCustomer.CustomerBirthday = EditingCustomer.CustomerBirthday;
                        existingCustomer.CustomerStatus = EditingCustomer.CustomerStatus;
                        
                        // Only update password if a new one is provided
                        if (!string.IsNullOrWhiteSpace(EditingCustomer.Password))
                        {
                            existingCustomer.Password = EditingCustomer.Password;
                        }
                        
                        _customerService.Update(existingCustomer);
                        MessageBox.Show("Customer updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }

                CancelEdit();
                LoadCustomers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving customer: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanSaveCustomer()
        {
            return EditingCustomer != null && IsEditing;
        }

        private void CancelEdit()
        {
            IsEditing = false;
            EditingCustomer = null;
            SelectedCustomer = null;
        }

        private void DeleteCustomer()
        {
            if (SelectedCustomer == null) return;

            var result = MessageBox.Show(
                $"Are you sure you want to delete customer '{SelectedCustomer.CustomerFullName}'?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _customerService.Delete(SelectedCustomer.CustomerID);
                    MessageBox.Show("Customer deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadCustomers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting customer: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool CanDeleteCustomer()
        {
            return SelectedCustomer != null && !IsEditing;
        }
        #endregion
    }
}
