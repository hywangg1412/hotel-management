using System.ComponentModel.DataAnnotations;

namespace FUMini.BussinessObjects.Models
{
    public class BookingReservation
    {
        // Primary Key
        [Key]
        public int BookingReservationID { get; set; }

        public DateOnly? BookingDate { get; set; }

        public decimal? TotalPrice { get; set; }

        public byte? BookingStatus { get; set; }

        // Foreign Key
        [Required]
        public int CustomerID { get; set; }

        // Navigation Properties
        public virtual Customer Customer { get; set; }

        // Navigation Collections
        public virtual IEnumerable<BookingDetail> BookingDetails { get; set; }

        public BookingReservation() { }

        public BookingReservation(int bookingReservationID, DateOnly? bookingDate, decimal? totalPrice, byte? bookingStatus, int customerID)
        {
            BookingReservationID = bookingReservationID;
            BookingDate = bookingDate;
            TotalPrice = totalPrice;
            BookingStatus = bookingStatus;
            CustomerID = customerID;
        }
    }
}
