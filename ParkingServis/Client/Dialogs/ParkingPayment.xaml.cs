using Microsoft.Extensions.DependencyInjection;
using ParkingServis.Client.Views;
using ParkingServis.Server.Services.ParkingSessionServices.Command;
using ParkingServis.Server.Services.UserServices.Command;
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
    /// Interaction logic for ParkingPayment.xaml
    /// </summary>
    public partial class ParkingPayment : Window
    {
        public static string RegNumber { get; set; }
        public static string LocationName { get; set; }
        public static DateTime parkingStart { get; set; }
        public static int sessionId { get; set; }
        decimal priceToPay;
        DateTime endPark;
        public event EventHandler ReloadHome;
        private readonly IParkingSessionCommandRepository _parkingSessionCommandRepository;
        private readonly UserCommandInterface _userCommandRepository;
        public ParkingPayment(IParkingSessionCommandRepository parkingSessionCommandRepository, UserCommandInterface userCommand)
        {
            InitializeComponent();
            _parkingSessionCommandRepository = parkingSessionCommandRepository;
            _userCommandRepository = userCommand;
            ParkingLocationTextBlock.Text = LocationName;
            VehicleRegNumberTextBlock.Text = RegNumber;
            DateOfParkStartTextBlock.Text = parkingStart.ToString();
            DateOfParkEndTextBlock.Text = DateTime.Now.ToString();
            UserNameTextBlock.Text =
                $"{Globals.CurrentUser.FirstName} {Globals.CurrentUser.LastName}";

            endPark = DateTime.Now;
            TimeSpan timeDiff = endPark - parkingStart;
            decimal hours = timeDiff.Hours + 1;

            priceToPay = hours * 50;

            PriceToPayTextBlock.Text = $"{priceToPay} RSD";
        }

        private async void PayByCardButton_Click(object sender, RoutedEventArgs e)
        {
            bool isPaid = await _parkingSessionCommandRepository.PayParkingSession(
                sessionId,
                priceToPay,
                endPark
            );
            if (isPaid)
            {
                MessageBox.Show("Naplata uspesna");
                _userCommandRepository.UpdateUserCredits(priceToPay, Globals.CurrentUser.Id);
                Globals.CurrentUser.Credits -= priceToPay; 
                ReloadHome?.Invoke(this, EventArgs.Empty);

                //send bill email
            }
        }
    }
}
