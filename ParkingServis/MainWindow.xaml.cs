using Microsoft.Extensions.DependencyInjection;
using ParkingServis.Client;
using ParkingServis.Client.EmailConfig;
using ParkingServis.Client.ViewModels;
using ParkingServis.Client.Views;
using ParkingServis.Server.Controllers.UserControllers;
using ParkingServis.Server.Database;
using ParkingServis.Server.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ParkingServis
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly UserCommandController _userController;
        private readonly UserQueryController _userQueryController;
        private readonly IServiceProvider _serviceProvider;
        public MainWindow(UserCommandController userController, UserQueryController userQueryController, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _userController = userController;
            _userQueryController = userQueryController;
            _serviceProvider = serviceProvider; 
           Client.ServiceProvider.serviceProvider = serviceProvider;
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

        private async void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            if(emailTb.Text != "" && passTb.Password.Length != 0)
            {

              User user = await _userQueryController.GetUserByEmailPassword(emailTb.Text, passTb.Password);
                if (user.Id > 0)
                {

                    Globals.CurrentUser = user;
                    HomeWindow home = _serviceProvider.GetRequiredService<HomeWindow>();
                    home.Show();
                    this.Close();
                }
                    
                else
                {
                    MessageNotification messageNotification = new MessageNotification();
                    messageNotification.HandleMessagesNotification(clientSB, "Email ili lozinka nisu tacni, pokusajte ponovo", 6, Brushes.IndianRed);
                }
            }
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void registerBtn_Click(object sender, RoutedEventArgs e)
        {


            RegistrationWindow registerWindow = _serviceProvider.GetRequiredService<RegistrationWindow>();
            registerWindow.Show();
        }
    }
}