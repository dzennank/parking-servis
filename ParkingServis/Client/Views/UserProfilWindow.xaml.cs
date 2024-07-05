using Microsoft.Extensions.DependencyInjection;
using ParkingServis.Client.Dialogs;
using ParkingServis.Server.Controllers.UserControllers;
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
    /// Interaction logic for UserProfilWindow.xaml
    /// </summary>
    public partial class UserProfilWindow : Window
    {
        private readonly UserCommandController _userController;
        public UserProfilWindow(UserCommandController userController)
        {
            InitializeComponent();
            _userController = userController;
            ImplementUserData();
        }

        private void ImplementUserData()
        {
            FirstNameTextBlock.Text = Globals.CurrentUser.FirstName;
            LastNameTextBlock.Text = Globals.CurrentUser.LastName;
            EmailTextBlock.Text = Globals.CurrentUser.Email;
            AdressTextBlock.Text = Globals.CurrentUser.Adress;
        }

        private async Task<bool> UpdateUser()
        {
            User updatedUser = new User
            {
                Id = Globals.CurrentUser.Id,
                FirstName = FirstNameTextBlock.Text,
                LastName = LastNameTextBlock.Text,
                Email = EmailTextBlock.Text,
                Adress = AdressTextBlock.Text,
            };

           return await _userController.UpdateUser(updatedUser);
        }

        private async void PayByCardButton_Click(object sender, RoutedEventArgs e)
        {
            bool isUserUpdated = await UpdateUser();
            if (isUserUpdated)
            {
                MessageBox.Show("Uspesno ste izmenili profil", "Izmena profila", MessageBoxButton.OK, MessageBoxImage.Information);
                Globals.CurrentUser.FirstName = FirstNameTextBlock.Text;
                Globals.CurrentUser.LastName = LastNameTextBlock.Text;
                Globals.CurrentUser.Email = EmailTextBlock.Text;
                Globals.CurrentUser.Adress = AdressTextBlock.Text;
                this.Close();
            }
            else
            {
                MessageBox.Show("Doslo je do greske, profil nece biti izmenjen", "Izmena profila", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void creditsPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           CardPayment window =
             ServiceProvider.serviceProvider.GetRequiredService<CardPayment>();
            window.Show();
        }
    }
}
