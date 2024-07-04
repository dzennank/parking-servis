using Microsoft.Extensions.DependencyInjection;
using ParkingServis.Client.Dialogs;
using ParkingServis.Server.Controllers.VehicleControllers;
using ParkingServis.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ParkingServis.Client.Views
{
    /// <summary>
    /// Interaction logic for VehiclesWindow.xaml
    /// </summary>
    public partial class VehiclesWindow : Window
    {
        private readonly VehicleQueryController _vehicleQueryController;
        private readonly IServiceProvider _serviceProvider;
        List<Vehicle> vehicles = new List<Vehicle>();
        public VehiclesWindow(VehicleQueryController vehicleQueryController, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _vehicleQueryController = vehicleQueryController;
            GetVehicles();
            _serviceProvider = serviceProvider;
        }

        private void GetVehicles()
        {
            vehicles =  _vehicleQueryController.GetVehiclesByUserId(Globals.CurrentUser.Id); 
            vehiclesDataGrid.ItemsSource = vehicles;
        }

        private void PayByCardButton_Click(object sender, RoutedEventArgs e)
        {
           AddVehicle window =
              _serviceProvider.GetRequiredService<AddVehicle>();
            window.Show();
        }
    }
}
