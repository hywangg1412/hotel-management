using FUMini.DataAccess.Repositories.Implementation;
using FUMini.DataAccess.Repositories.Interfaces;
using FUMini.Services.Implementations;
using FUMini.Services.Interfaces;
using FUMini.UI.View;
using FUMini.DataAccess.Context;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace FUMini.UI
{
    public partial class App : Application
    {
        private IServiceProvider serviceProvider;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            // DbContext
            services.AddDbContext<FUMiniContext>();

            // Repository
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IBookingReservationRepository, BookingReservationRepository>();
            services.AddScoped<IBookingDetailRepository, BookingDetailRepository>();
            services.AddScoped<IRoomInformationRepository, RoomInformationRepository>();
            services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();

            // Service
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IBookingReservationService, BookingReservationService>();
            services.AddScoped<IBookingDetailService, BookingDetailService>();
            services.AddScoped<IRoomInformationService, RoomInformationService>();
            services.AddScoped<IRoomTypeService, RoomTypeService>();

            // Window
            services.AddTransient<LoginWindow>();
            services.AddTransient<RegisterWindow>();
            services.AddTransient<MainWindow>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var loginWindow = serviceProvider.GetService<LoginWindow>();
            loginWindow.Show();
        }
    }
}
