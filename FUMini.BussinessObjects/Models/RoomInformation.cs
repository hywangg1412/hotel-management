using System.ComponentModel.DataAnnotations;

namespace FUMini.BussinessObjects.Models
{
    public class RoomInformation
    {
        // Primary Key
        [Key]
        public int RoomID { get; set; }

        // Normal Properties
        [Required, StringLength(50, ErrorMessage = "Room number cannot exceed 50 characters")]
        public string RoomNumber { get; set; }
        public string? RoomDetailDescription { get; set; }
        public int? RoomMaxCapacity { get; set; }
        public byte? RoomStatus { get; set; }
        public decimal? RoomPricePerDay { get; set; }

        // Foreign Key
        [Required]
        public int RoomTypeID { get; set; }

        // Navigation Properties
        public virtual RoomType RoomType { get; set; }

        // Navigation Collections
        public virtual IEnumerable<BookingDetail> BookingDetails { get; set; }

        public RoomInformation()
        {
        }

        public RoomInformation(int roomId, string roomNumber, string? roomDetailDescription, int? roomMaxCapacity, int roomTypeID, byte? roomStatus, decimal? roomPricePerDay)
        {
            RoomID = roomId;
            RoomNumber = roomNumber;
            RoomDetailDescription = roomDetailDescription;
            RoomMaxCapacity = roomMaxCapacity;
            RoomTypeID = roomTypeID;
            RoomStatus = roomStatus;
            RoomPricePerDay = roomPricePerDay;
        }
    }
}
