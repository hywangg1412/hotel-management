using FUMini.BussinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FUMini.DataAccess.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public CustomerConfiguration() { }

        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.CustomerID);

            // User - BookingReservations
            builder.HasMany(c => c.BookingReservations)
                .WithOne(br => br.Customer)
                .HasForeignKey(br => br.CustomerID);

            builder.ToTable("Customers");
        }
    }
}
