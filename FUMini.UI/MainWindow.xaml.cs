using System.Windows;

namespace FUMini.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BookingHistoryBtn_Click(object sender, RoutedEventArgs e)
        {
            // Xử lý khi click vào Booking History
            MessageBox.Show("Booking History clicked!");
        }

        private void BookingManagementBtn_Click(object sender, RoutedEventArgs e)
        {
            // Xử lý khi click vào Booking Management
            MessageBox.Show("Booking Management clicked!");
        }

        private void CustomerManagementBtn_Click(object sender, RoutedEventArgs e)
        {
            // Xử lý khi click vào Customer Management
            MessageBox.Show("Customer Management clicked!");
        }

        private void RoomManagementBtn_Click(object sender, RoutedEventArgs e)
        {
            // Xử lý khi click vào Room Management
            MessageBox.Show("Room Management clicked!");
        }

        private void ReportBtn_Click(object sender, RoutedEventArgs e)
        {
            // Xử lý khi click vào Reports & Statistics
            MessageBox.Show("Reports & Statistics clicked!");
        }

        private void ProfileBtn_Click(object sender, RoutedEventArgs e)
        {
            // Xử lý khi click vào My Profile
            MessageBox.Show("My Profile clicked!");
        }

        private void NewBookingBtn_Click(object sender, RoutedEventArgs e)
        {
            // Xử lý khi click vào New Booking
            MessageBox.Show("New Booking clicked!");
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Xử lý khi click vào Logout
            MessageBox.Show("Logout clicked!");
        }
    }
}