using FUMini.BussinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FUMini.DataAccess.Configurations
{
    public class BookingDetailConfiguration : IEntityTypeConfiguration<BookingDetail>
    {
        public void Configure(EntityTypeBuilder<BookingDetail> builder)
        {
            // ID
            builder.HasKey(bd => new { bd.BookingReservationID, bd.RoomID });

            // Booking Details - Booking Reservation
            builder.HasOne(br => br.BookingReservation)
                .WithMany(bd => bd.BookingDetails)
                .HasForeignKey(br => br.BookingReservationID);

            // Booking Details - RoomInformation
            builder.HasOne(ri => ri.RoomInformation)
                .WithMany(bd => bd.BookingDetails)
                .HasForeignKey(ri => ri.RoomID);
        }
    }
}
