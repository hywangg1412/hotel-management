using FUMini.BussinessObjects.Models;
using FUMini.Services.Interfaces;
using FUMini.UI.ViewModel.Base;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace FUMini.UI.ViewModel.Admin
{
    public class ReportViewModel : BaseViewModel
    {
        private readonly IBookingReservationService _bookingReservationService;
        private readonly IBookingDetailService _bookingDetailService;
        private ObservableCollection<BookingReservation> _reportData;
        private DateTime _startDate;
        private DateTime _endDate;
        private decimal _totalRevenue;
        private int _totalBookings;
        private int _totalRoomsBooked;

        public ReportViewModel(
            IBookingReservationService bookingReservationService,
            IBookingDetailService bookingDetailService)
        {
            _bookingReservationService = bookingReservationService;
            _bookingDetailService = bookingDetailService;
            
            // Set default date range (last 30 days)
            EndDate = DateTime.Today;
            StartDate = EndDate.AddDays(-30);
            
            InitializeCommands();
        }

        #region Properties
        public ObservableCollection<BookingReservation> ReportData
        {
            get => _reportData;
            set => SetProperty(ref _reportData, value);
        }

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                SetProperty(ref _startDate, value);
                if (EndDate < StartDate)
                {
                    EndDate = StartDate.AddDays(1);
                }
            }
        }

        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                SetProperty(ref _endDate, value);
                if (StartDate > EndDate)
                {
                    StartDate = EndDate.AddDays(-1);
                }
            }
        }

        public decimal TotalRevenue
        {
            get => _totalRevenue;
            set => SetProperty(ref _totalRevenue, value);
        }

        public int TotalBookings
        {
            get => _totalBookings;
            set => SetProperty(ref _totalBookings, value);
        }

        public int TotalRoomsBooked
        {
            get => _totalRoomsBooked;
            set => SetProperty(ref _totalRoomsBooked, value);
        }
        #endregion

        #region Commands
        public ICommand GenerateReportCommand { get; private set; }
        public ICommand ExportReportCommand { get; private set; }
        #endregion

        #region Methods
        private void InitializeCommands()
        {
            GenerateReportCommand = new RelayCommand(GenerateReport);
            ExportReportCommand = new RelayCommand(ExportReport, CanExportReport);
        }

        private void GenerateReport()
        {
            try
            {
                if (StartDate > EndDate)
                {
                    MessageBox.Show("Start date cannot be after end date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Get all bookings within the date range
                var allBookings = _bookingReservationService.GetAll();
                var filteredBookings = allBookings
                    .Where(b => b.BookingDate >= DateOnly.FromDateTime(StartDate) && 
                               b.BookingDate <= DateOnly.FromDateTime(EndDate))
                    .OrderByDescending(b => b.BookingDate)
                    .ToList();

                ReportData = new ObservableCollection<BookingReservation>(filteredBookings);

                // Calculate statistics
                CalculateStatistics(filteredBookings);

                MessageBox.Show($"Report generated successfully!\n" +
                              $"Period: {StartDate:dd/MM/yyyy} - {EndDate:dd/MM/yyyy}\n" +
                              $"Total Bookings: {TotalBookings}\n" +
                              $"Total Revenue: {TotalRevenue:C}\n" +
                              $"Total Rooms Booked: {TotalRoomsBooked}",
                              "Report Generated", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating report: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CalculateStatistics(List<BookingReservation> bookings)
        {
            TotalBookings = bookings.Count;
            TotalRevenue = bookings.Sum(b => b.TotalPrice ?? 0);

            // Calculate total rooms booked
            var bookingIds = bookings.Select(b => b.BookingReservationID).ToList();
            var allBookingDetails = _bookingDetailService.GetAll();
            var relevantDetails = allBookingDetails.Where(bd => bookingIds.Contains(bd.BookingReservationID)).ToList();
            TotalRoomsBooked = relevantDetails.Count;
        }

        private void ExportReport()
        {
            try
            {
                if (ReportData == null || !ReportData.Any())
                {
                    MessageBox.Show("No report data to export. Please generate a report first.", "No Data", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var fileName = $"Hotel_Report_{StartDate:yyyyMMdd}_{EndDate:yyyyMMdd}.txt";
                var filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);

                using (var writer = new System.IO.StreamWriter(filePath))
                {
                    writer.WriteLine("HOTEL MANAGEMENT SYSTEM - REPORT");
                    writer.WriteLine("=================================");
                    writer.WriteLine($"Report Period: {StartDate:dd/MM/yyyy} - {EndDate:dd/MM/yyyy}");
                    writer.WriteLine($"Generated on: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
                    writer.WriteLine();
                    writer.WriteLine($"Total Bookings: {TotalBookings}");
                    writer.WriteLine($"Total Revenue: {TotalRevenue:C}");
                    writer.WriteLine($"Total Rooms Booked: {TotalRoomsBooked}");
                    writer.WriteLine();
                    writer.WriteLine("BOOKING DETAILS:");
                    writer.WriteLine("================");

                    foreach (var booking in ReportData)
                    {
                        writer.WriteLine($"Booking ID: {booking.BookingReservationID}");
                        writer.WriteLine($"Customer: {booking.Customer?.CustomerFullName ?? "N/A"}");
                        writer.WriteLine($"Booking Date: {booking.BookingDate:dd/MM/yyyy}");
                        writer.WriteLine($"Total Price: {booking.TotalPrice:C}");
                        writer.WriteLine($"Status: {GetBookingStatusText(booking.BookingStatus ?? 0)}");
                        writer.WriteLine("---");
                    }
                }

                MessageBox.Show($"Report exported successfully to:\n{filePath}", "Export Successful", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting report: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanExportReport()
        {
            return ReportData != null && ReportData.Any();
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
