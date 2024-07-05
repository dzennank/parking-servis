using Microsoft.Extensions.DependencyInjection;
using ParkingServis.Server.Controllers.LocationControllers;
using ParkingServis.Server.Models;
using ParkingServis.Server.Services.LocationServices.Query;
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
    /// Interaction logic for LocationsWindow.xaml
    /// </summary>
    public partial class LocationsWindow : Window
    {
        private readonly LocationQueryController _locationQueryController;
        List<Location> locations;
        public static HomeWindow HomeWindow { get; set; }
        public LocationsWindow(LocationQueryController locationQueryRepository)
        {
            InitializeComponent();
            _locationQueryController = locationQueryRepository;
            GetLocations();
            LocationDataGrid.ItemsSource = locations;
        }

        private async void GetLocations()
        {
            locations = await _locationQueryController.GetAllLocations();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Location selectedLocation = (sender as Button).DataContext as Location;
            LocationAdminDetails.locationId = selectedLocation.Id;
            LocationAdminDetails.HomeWindow = HomeWindow;
            LocationAdminDetails locationAdminDetails = Client.ServiceProvider.serviceProvider.GetRequiredService<LocationAdminDetails>();
            locationAdminDetails.Show();
            
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(searchTb.Text.Length >= 2)
            {
                LocationDataGrid.ItemsSource = filterLocations(searchTb.Text);
            }
            else
            {
                LocationDataGrid.ItemsSource = locations;
            }
        }

        private List<Location> filterLocations(string txt)
        {

            List<Location> filteredLocations = locations.Where(loc => loc.Name.ToLower().Contains(txt.ToLower())).ToList();
            return filteredLocations;

        }
    }
}