using System.ComponentModel.DataAnnotations;

namespace FUMini.BussinessObjects.Models
{
    public class Customer
    {
        // Primary Key
        [Key]
        public int CustomerID { get; set; }

        // Normal Properties
        [StringLength(50, ErrorMessage = "Fullname cannot exceed 50 characters")]
        public string CustomerFullName { get; set; }

        [StringLength(12, ErrorMessage = "Phone number cannot exceed 12 characters")]
        public string Telephone { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Email cannot exceed 50 characters")]
        public string EmailAddress { get; set; }

        public DateOnly? CustomerBirthday { get; set; }

        public byte? CustomerStatus { get; set; }

        [StringLength(50, ErrorMessage = "Password cannot exceed 50 character")]
        public string Password { get; set; }

        // Navigation Properties

        // Navigation Collections
        public virtual IEnumerable<BookingReservation> BookingReservations { get; set; }

        public Customer() { }

        public Customer(int customerID, string customerFullName, string telephone, string emailAddress, DateOnly? customerBirthday, byte? customerStatus, string password)
        {
            CustomerID = customerID;
            CustomerFullName = customerFullName;
            Telephone = telephone;
            EmailAddress = emailAddress;
            CustomerBirthday = customerBirthday;
            CustomerStatus = customerStatus;
            Password = password;
        }
    }
}


