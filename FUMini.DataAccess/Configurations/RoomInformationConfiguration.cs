using FUMini.BussinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FUMini.DataAccess.Configurations
{
    public class RoomInformationConfiguration : IEntityTypeConfiguration<RoomInformation>
    {
        public void Configure(EntityTypeBuilder<RoomInformation> builder)
        {
            // ID
            builder.HasKey(ri => ri.RoomID);

            // RoomInformation - BookingDetail
            builder.HasMany(bd => bd.BookingDetails)
                .WithOne(ri => ri.RoomInformation)
                .HasForeignKey(ri => ri.RoomID);

            // RoomType - RoomInformation
            builder.HasOne(rt => rt.RoomType)
                .WithMany(ri => ri.RoomInformations)
                .HasForeignKey(rt => rt.RoomTypeID);
        }
    }
}