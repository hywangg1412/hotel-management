using FUMini.BussinessObjects.Models;
using FUMini.DataAccess.Context;
using FUMini.DataAccess.Repositories.Implementation;
using FUMini.DataAccess.Repositories.Interfaces;
using FUMini.Services.Implementations;
using FUMini.Services.Interfaces;
using FUMini.UI.View;
using FUMini.UI.ViewModel;
using FUMini.UI.ViewModel.Admin;
using FUMini.UI.ViewModel.Auth;
using FUMini.UI.ViewModel.User;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace FUMini.UI
{
    public partial class App : Application
    {
        private readonly IServiceProvider serviceProvider;

        public IServiceProvider Services => serviceProvider;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var adminConfig = configuration.GetSection("AdminAccount").Get<AdminConfig>();

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
            
            // ViewModels
            services.AddTransient<LoginViewModel>();
            services.AddTransient<RegisterViewModel>();
            services.AddTransient<AdminDashboardViewModel>();
            services.AddTransient<UserDashboardViewModel>();
            services.AddTransient<CustomerManagementViewModel>();
            services.AddTransient<RoomManagementViewModel>();
            services.AddTransient<BookingManagementViewModel>();
            services.AddTransient<ReportViewModel>();
            services.AddTransient<ProfileViewModel>();
            services.AddTransient<CreateBookingViewModel>();
            services.AddTransient<BookingHistoryViewModel>();
            services.AddTransient<UpdateProfileViewModel>();
            services.AddTransient<EditBookingViewModel>();
            services.AddTransient<BookingDetailsViewModel>();

            // Admin Account
            services.AddSingleton(adminConfig);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var loginWindow = serviceProvider.GetRequiredService<LoginWindow>();
            loginWindow.Show();
        }
    }
}
