using Microsoft.Extensions.DependencyInjection;
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
    /// Interaction logic for RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        private readonly UserCommandController _userController;
        private readonly IServiceProvider _serviceProvider;
        public RegistrationWindow(UserCommandController userController, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _userController = userController;
            _serviceProvider = serviceProvider;
        }

        private void nameTblock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            nameTb.Focus();
        }

        private void nameTb_GotFocus(object sender, RoutedEventArgs e)
        {
            nameTblock.Visibility = Visibility.Collapsed;
            nameTblock.Text = "";
        }

        private void nameTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(nameTb.Text))
            {
                nameTblock.Visibility = Visibility.Collapsed;
                nameTblock.Text = "";

            }
            else
            {
                nameTblock.Visibility = Visibility.Visible;
                nameTblock.Text = "ime";
            }
        }

        private void LastnameTblock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LastnameTb.Focus();
        }

        private void LastnameTb_GotFocus(object sender, RoutedEventArgs e)
        {
            LastnameTblock.Visibility = Visibility.Collapsed;
            LastnameTblock.Text = "";
        }

        private void LastnameTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(LastnameTb.Text))
            {
                LastnameTblock.Visibility = Visibility.Collapsed;
                LastnameTblock.Text = "";

            }
            else
            {
                LastnameTblock.Visibility = Visibility.Visible;
                LastnameTblock.Text = "Prezime";
            }
        }

        private void addressTblock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            adressTb.Focus();
        }

        private void adressTb_GotFocus(object sender, RoutedEventArgs e)
        {
            addressTblock.Visibility = Visibility.Collapsed;
            addressTblock.Text = "";
        }

        private void adressTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(adressTb.Text))
            {
                addressTblock.Visibility = Visibility.Collapsed;
                addressTblock.Text = "";

            }
            else
            {
                addressTblock.Visibility = Visibility.Visible;
                addressTblock.Text = "Adresa";
            }
        }

        private void emailTblock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            emailTb.Focus();

        }

        private void passTblock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            passTb.Focus();
            passTblock.Text = "";
        }





        private void emailTb_GotFocus(object sender, RoutedEventArgs e)
        {
            emailTblock.Visibility = Visibility.Collapsed;
            emailTblock.Text = "";
        }

        private void emailTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(emailTb.Text) && emailTb.Text.Length > 0)
            {
                emailTblock.Visibility = Visibility.Collapsed;
                emailTblock.Text = "";
            }
            else
            {
                emailTblock.Visibility = Visibility.Visible;
                emailTblock.Text = "Email";
            }
        }

        private void passTb_GotFocus(object sender, RoutedEventArgs e)
        {
            passTblock.Visibility = Visibility.Collapsed;
            passTblock.Text = "";
        }

        private void passTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(passTb.Password) && passTb.Password.Length > 0)
            {
                passTblock.Visibility = Visibility.Collapsed;
                passTblock.Text = "";

            }
            else
            {
                passTblock.Visibility = Visibility.Visible;
                passTblock.Text = "Lozinka";
            }
        }

        private async void registerBtn_Click(object sender, RoutedEventArgs e)
        {
            string email = emailTb.Text;
            string password = passTb.Password;
            MessageNotification messageNotification = new MessageNotification();
            if(string.IsNullOrEmpty(nameTb.Text))
                messageNotification.HandleMessagesNotification(clientSB, "Polje Ime ne sme niti prazno", 4, Brushes.IndianRed);

            if (string.IsNullOrEmpty(LastnameTb.Text))
                messageNotification.HandleMessagesNotification(clientSB, "Polje Prezime ne sme niti prazno", 4, Brushes.IndianRed);

            if (string.IsNullOrEmpty(adressTb.Text))
                messageNotification.HandleMessagesNotification(clientSB, "Polje Adresa ne sme niti prazno", 4, Brushes.IndianRed);

            if (!IsValidEmail(email))
            {
                messageNotification.HandleMessagesNotification(clientSB, "Uneiste validan mail", 4, Brushes.IndianRed);
            }
            else if (!IsValidPassword(password))
            {
               
                clientSB.FontSize = 11;
                messageNotification.HandleMessagesNotification(clientSB, "Lozinka mora biti najmanje 6 karaktera, jedno veliko slovo i specijalni karakter", 6, Brushes.IndianRed);
            }
            else
            {
                User newUser = new User
                {
                    FirstName = nameTb.Text,
                    LastName = LastnameTb.Text,
                    Email = emailTb.Text,
                    Password = passTb.Password,
                    Adress = adressTb.Text,
                };
               bool isUserCreated = await _userController.CreateUser(newUser);
                if (isUserCreated)
                {
                    MainWindow loginWindow = _serviceProvider.GetRequiredService<MainWindow>();
                    loginWindow.Show();
                    this.Close();
                }
            }

            
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }


        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPassword(string password)
        {
            if (password.Length < 6)
                return false;

            bool hasUpperCase = false;
            bool hasSymbol = false;

            foreach (char c in password)
            {
                if (char.IsUpper(c))
                    hasUpperCase = true;

                if (!char.IsLetterOrDigit(c))
                    hasSymbol = true;
            }

            return hasUpperCase && hasSymbol;
        }

        private void returnToLogInBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow loginWindow = _serviceProvider.GetRequiredService<MainWindow>();
            loginWindow.Show();
            this.Close();
        }
    }
}
