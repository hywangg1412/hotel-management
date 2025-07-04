using FUMini.BussinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FUMini.DataAccess.Configurations
{
    public class BookingReservationConfiguration : IEntityTypeConfiguration<BookingReservation>
    {
        public void Configure(EntityTypeBuilder<BookingReservation> builder)
        {
            builder.HasKey(b => b.BookingReservationID);

            // RoomReservation - User 
            builder.HasOne(c => c.Customer)
                .WithMany(br => br.BookingReservations)
                .HasForeignKey(c => c.CustomerID);
        }
    }
}
