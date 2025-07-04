using System.ComponentModel.DataAnnotations;

namespace FUMini.BussinessObjects.Models
{
    public class BookingDetail
    {
        // Composite Primary Key
        [Key]
        [Required]
        public int BookingReservationID { get; set; }

        [Key]
        [Required]
        public int RoomID { get; set; }

        // Normal Properties
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public decimal? ActualPrice { get; set; }

        // Navigation Properties
        public virtual RoomInformation RoomInformation { get; set; }
        public virtual BookingReservation BookingReservation { get; set; }

        // Navigation Collection


        public BookingDetail() { }

        public BookingDetail(int bookingReservationID, int roomID, DateOnly startDate, DateOnly endDate, decimal? actualPrice)
        {
            BookingReservationID = bookingReservationID;
            RoomID = roomID;
            StartDate = startDate;
            EndDate = endDate;
            ActualPrice = actualPrice;
        }

    }
}
