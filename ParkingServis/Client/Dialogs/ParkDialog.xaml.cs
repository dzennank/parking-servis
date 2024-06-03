using ParkingServis.Server.Controllers.ParkingSessionController;
using ParkingServis.Server.Controllers.VehicleControllers;
using ParkingServis.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
using static MaterialDesignThemes.Wpf.Theme;

namespace ParkingServis.Client.Dialogs
{
    /// <summary>
    /// Interaction logic for ParkDialog.xaml
    /// </summary>
    public partial class ParkDialog : Window
    {
        private readonly VehicleQueryController _vehicleQueryController;
        private readonly ParkingSessionQueryController _parkingSessionQueryController;
        private readonly ParkingcSessionCommandController _parkingcSessionCommandController;
        List<Vehicle> vehicles;
        public event EventHandler LocationUpdated;

        public ParkDialog(
            VehicleQueryController vehicleQueryController,
            ParkingcSessionCommandController parkingcSessionCommandController,
            ParkingSessionQueryController parkingSessionQueryController
        )
        {
            InitializeComponent();
            _vehicleQueryController = vehicleQueryController;
            _parkingcSessionCommandController = parkingcSessionCommandController;
            _parkingSessionQueryController = parkingSessionQueryController;
            vehicles = _vehicleQueryController.GetVehiclesByUserId(Globals.CurrentUser.Id);

            foreach (var vehicle in vehicles)
            {
                ComboBoxItem cbi = new ComboBoxItem();
                cbi.Content = vehicle.RegNumber;
                vehicleListCb.Items.Add(cbi);
            }
        }

        private async void parkBtn_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem selectedItem = vehicleListCb.SelectedItem as ComboBoxItem;
            bool sesionExists = await CheckIfSessionExists(selectedItem.Content.ToString());
            if(selectedItem != null && !sesionExists)
            {
                ParkingSession session = new ParkingSession
                {
                    UserId = Globals.CurrentUser.Id,
                    LocationId = Globals.ClickedLocation.Id,
                    VehicleReg = selectedItem.Content.ToString(),
                    ParkingStart = DateTime.Now,
                    PaymentStatus = false
                };
                bool isSessionAdded = await _parkingcSessionCommandController.AddParkingSession(
                    session
                );
                if (isSessionAdded)
                {
                    Globals.ParkedVehicleRegNumber = selectedItem.Content.ToString();
                    LocationUpdated?.Invoke(this, EventArgs.Empty);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Doslo je do greske");
                }
            }
            else
            {
                MessageBox.Show($"Vec ste parkirali vozilo {selectedItem.Content.ToString()}, pokusajte sa drugim vozilom");
            }
            
        }

        private async Task<bool> CheckIfSessionExists(string regNumber)
        {
            List<ParkingSession> parkingSessions = await _parkingSessionQueryController.GetSessionsByUserId(Globals.CurrentUser.Id);
            if(parkingSessions.Count > 0 )
            {
                foreach( var parkingSession in parkingSessions )
                {
                    if(parkingSession.PaymentStatus == false && parkingSession.VehicleReg == regNumber)
                    {                        
                        return true;
                    }
                   
                }
                return false;
            }
            else
            {
                return false;
            }
        }
    }
}
