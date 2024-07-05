using ParkingServis.Server.Controllers.ParkingSessionController;
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
using ParkingServis.Server.Models;
using ParkingServis.Server.Controllers.LocationControllers;

namespace ParkingServis.Client.Views
{
    /// <summary>
    /// Interaction logic for TransactionsWindow.xaml
    /// </summary>
    public partial class TransactionsWindow : Window
    {
        private readonly ParkingSessionQueryController _parkingSessionQueryController;
        List<ParkingSession> sessionsList = new List<ParkingSession>();
        public TransactionsWindow(ParkingSessionQueryController parkingSessionQueryController)
        {
            InitializeComponent();
            _parkingSessionQueryController = parkingSessionQueryController;
            if(Globals.CurrentUser.Role != "admin")
            {
                GetSessions();

            } 
            else
            {
                GetSessionsAdmin();
            }


        }

        private async void GetSessions()
        {
            sessionsList = await _parkingSessionQueryController.GetSessionsByUserId(Globals.CurrentUser.Id, 1);
            transactionDataGrid.ItemsSource = sessionsList;
            totalSessionsLb.Content = sessionsList.Count.ToString();
            totalSpentLb.Content = sessionsList.Sum(s => s.PricePaid).ToString();
        }

        private void GetSessionsAdmin()
        {
            sessionsList = _parkingSessionQueryController.GetParkingSessions();
            transactionDataGrid.ItemsSource = sessionsList;
            totalSessionsLb.Content = sessionsList.Count.ToString();
            totalSpentLb.Content = sessionsList.Sum(s => s.PricePaid).ToString();
        }
    }
}
