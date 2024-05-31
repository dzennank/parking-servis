using Microsoft.Extensions.DependencyInjection;
using ParkingServis.Client.ViewModels;
using ParkingServis.Server.Controllers.UserControllers;
using ParkingServis.Server.Services.UserServices.Command;
using ParkingServis.Server.Services.UserServices.Query;
using System.Configuration;
using System.Data;
using ParkingServis.Client.Views;
using System.Windows;
using ParkingServis.Server.Services.LocationServices.Query;
using ParkingServis.Server.Controllers.LocationControllers;
using ParkingServis.Server.Services.VehicleServices.Command;
using ParkingServis.Server.Services.VehicleServices.Query;
using ParkingServis.Server.Controllers.VehicleControllers;
using ParkingServis.Client.Dialogs;
using ParkingServis.Server.Services.ParkingSessionServices.Command;
using ParkingServis.Server.Controllers.ParkingSessionController;
using ParkingServis.Server.Services.ParkingSessionServices.Query;

namespace ParkingServis
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
           
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Register your services and repositories here
            services.AddTransient<UserCommandInterface, UserCommandRepository>();
            services.AddTransient<UserCommandController>();
            services.AddTransient<UserQueryInterface, UserQueryRepository>();
            services.AddTransient<UserQueryController>();

            //Location services register
            services.AddTransient<ILocationQueryRepository, LocationQueryRepository>();
            services.AddTransient<LocationQueryController>();

            //Vehicle services register
            services.AddTransient<IVehicleQueryRepository, VehicleQueryRepository>();
            services.AddTransient<VehicleQueryController>();

            //ParkingSession services register
            services.AddTransient<IParkingSessionCommandRepository, ParkingSessionCommandRepository>();
            services.AddTransient<ParkingcSessionCommandController>();
            services.AddTransient<IParkingSessionQueryRepository, ParkingSessionQueryRepository>();
            services.AddTransient<ParkingSessionQueryController>();

            services.AddSingleton<MainWindow>();
            services.AddTransient<RegistrationWindow>();
            services.AddTransient<HomeWindow>();
            services.AddTransient<ParkDialog>();
        }
    }

}
