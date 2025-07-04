using FUMini.BussinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FUMini.DataAccess.Context
{
    public class FUMiniContext : DbContext
    {
        public FUMiniContext() { }

        public FUMiniContext(DbContextOptions options) : base(options) { }

        private string GetConnectionString()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            return configuration.GetConnectionString("DefaultConnection");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies().UseSqlServer(GetConnectionString());
        }

        // DBSet properties
        public DbSet<Customer> Customers { get; set; }
        public DbSet<BookingReservation> BookingReservations { get; set; }
        public DbSet<BookingDetail> BookingDetails { get; set; }
        public DbSet<RoomInformation> RoomInformations { get; set; }
        public DbSet<RoomType> roomTypes { get; set; }

        // Configure relationship
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FUMiniContext).Assembly);
        }
    }
}
