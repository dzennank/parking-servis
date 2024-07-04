using Microsoft.Extensions.DependencyInjection;
using ParkingServis.Client.Dialogs;
using ParkingServis.Server.Controllers.ParkingSessionController;
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
    /// Interaction logic for LocationAdminDetails.xaml
    /// </summary>
    public partial class LocationAdminDetails : Window
    {
        private readonly ParkingSessionQueryController _parkingSessionQueryController;
        List<ParkingSession> parkingSessions;
        public static  int locationId { get; set; }
        public static HomeWindow HomeWindow { get; set; }
        public LocationAdminDetails(ParkingSessionQueryController parkingSessionQueryController)
        {
            InitializeComponent();
            _parkingSessionQueryController = parkingSessionQueryController;
            GetSessions(locationId);
            
        }

        private async void GetSessions(int id)
        {
            parkingSessions = await _parkingSessionQueryController.GetSessionsByLocationId(id);
            sessionDataGrid.ItemsSource = parkingSessions;
         }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ParkingSession selectedParkingSession = (sender as Button).DataContext as ParkingSession;
            ParkingPaymentByAdmin.CurrentSession = selectedParkingSession;
            ParkingPaymentByAdmin.HomeWindow = HomeWindow;
            ParkingPaymentByAdmin pay = ServiceProvider.serviceProvider.GetRequiredService<ParkingPaymentByAdmin>();
            pay.Show();
        }
    }
}
