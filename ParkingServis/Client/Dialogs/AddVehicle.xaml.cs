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

namespace ParkingServis.Client.Dialogs
{
    /// <summary>
    /// Interaction logic for AddVehicle.xaml
    /// </summary>
    public partial class AddVehicle : Window
    {
        private readonly VehicleCommandController _vehicleCommandController;
        public AddVehicle(VehicleCommandController vehicleCommandController)
        {
            _vehicleCommandController = vehicleCommandController;
            InitializeComponent();
        }

        private async void AddVehicleButton_Click(object sender, RoutedEventArgs e)
        {
            if(RegTextBlock.Text != string.Empty)
            {
                if(int.TryParse(YearTextBlock.Text, out int prodYear))
                {
                    bool isCreated = await CreateVehicle();
                    if (isCreated)
                    {
                        MessageBox.Show("Uspesno ste dodali vozilo", "Dodavanje vozila", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Doslo je do greske", "Dodavanje vozila", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Godina proizvodnje mora biti broj", "Dodavanje vozila", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Registraciona oznaka polje ne sme biti prazno", "Dodavanje vozila", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private async Task<bool> CreateVehicle()
        {
            Vehicle newVehicle = new Vehicle
            {
                RegNumber = RegTextBlock.Text,
                Brand = BrandTextBlock.Text,
                Model = ModelTextBlock.Text,
                Color = ColorTextBlock.Text,
                ProductionYear = Convert.ToInt32(YearTextBlock.Text)
            };

            return await _vehicleCommandController.CreateVehicle(newVehicle, Globals.CurrentUser.Id);
        }
    }
}
