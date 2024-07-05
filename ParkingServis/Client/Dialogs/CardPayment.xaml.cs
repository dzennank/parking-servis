using ParkingServis.Server.Controllers.UserControllers;
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
    /// Interaction logic for CardPayment.xaml
    /// </summary>
    public partial class CardPayment : Window
    {
        private readonly UserCommandController _userCommandController;
        public CardPayment(UserCommandController userCommandController)
        {
            InitializeComponent();
            _userCommandController = userCommandController;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(iznosTb.Text, out int credits))
            {
                bool isCreditsUpdated = await _userCommandController.UpdateUserCredits(credits, Globals.CurrentUser.Id);
                if (isCreditsUpdated)
                {
                    MessageBox.Show("Uspesno ste uplatili kredit", "Provera", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Unesite validan broj u polje Iznos", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
