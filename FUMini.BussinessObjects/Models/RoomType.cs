using System.ComponentModel.DataAnnotations;

namespace FUMini.BussinessObjects.Models
{
    public class RoomType
    {
        // Primary Key
        [Key]
        [Required]
        public int RoomTypeID { get; set; }

        // Normal Properties
        [Required, StringLength(50, ErrorMessage = "Room type name cannot exceed 50 characters")]
        public string RoomTypeName { get; set; }
        [StringLength(250, ErrorMessage = "Room type name cannot exceed 250 characters ")]
        public string? TypeDescription { get; set; }
        public string? TypeNote { get; set; }

        // Navigation Properties


        // Navigation Collections
        public virtual IEnumerable<RoomInformation> RoomInformations { get; set; }

        public RoomType() { }

        public RoomType(int roomTypeID, string roomTypeName, string? typeDescription, string? typeNote)
        {
            RoomTypeID = roomTypeID;
            RoomTypeName = roomTypeName;
            TypeDescription = typeDescription;
            TypeNote = typeNote;
        }
    }
}
