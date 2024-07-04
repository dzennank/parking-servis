using GMap.NET.MapProviders;
using GMap.NET;
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
using GMap.NET.WindowsPresentation;
using ParkingServis.Server.Controllers.LocationControllers;
using ParkingServis.Server.Models;
using ParkingServis.Client.Dialogs;
using Microsoft.Extensions.DependencyInjection;
using static GMap.NET.MapProviders.StrucRoads.SnappedPoint;
using ParkingServis.Server.Controllers.ParkingSessionController;
using System.Reflection;

namespace ParkingServis.Client.Views
{
    /// <summary>
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {
        private readonly LocationQueryController _locationQueryController;
        private readonly ParkingSessionQueryController _parkingSessionQueryController;
        private readonly IServiceProvider _serviceProvider;
        List<ParkingServis.Server.Models.Location> locations = new List<Server.Models.Location>();
        public static DateTime ReservationDate { get; set; }
        public HomeWindow(
            LocationQueryController locationQueryController,
            IServiceProvider serviceProvider,
            ParkingSessionQueryController parkingSessionQueryController
        )
        {
            InitializeComponent();
            _locationQueryController = locationQueryController;
            _parkingSessionQueryController = parkingSessionQueryController;
            MainMap.MapProvider = GMapProviders.GoogleMap;
            MainMap.Position = new PointLatLng(43.139833, 20.517584); // Example coordinates (Eiffel Tower)
            MainMap.MinZoom = 2;
            MainMap.MaxZoom = 17;
            MainMap.Zoom = 15;
            GetLocations();
            AddMarkerToLocations(locations);
            GenerateParkingLocationUI(locations);
            _serviceProvider = serviceProvider;
            creditsDataTb.Text = $"Stanje na racunu: {Globals.CurrentUser.Credits}";
            if (Globals.CurrentUser.Role != "admin")
            {
                allLocationsNavTb.Visibility = Visibility.Hidden;
            }
            else
            {
                vehiclesNavTb.Visibility = Visibility.Collapsed;
                Grid.SetRow(allLocationsNavTb, 2);
            }
        }

        private async void AddMarkerToLocations(List<Server.Models.Location> locations)
        {
            // Create a new marker
            foreach (Server.Models.Location location in locations)
            {
                PointLatLng position = new PointLatLng(location.CoordinateX, location.CoordinateY);
                GMapMarker marker = new GMapMarker(position)
                {
                    Shape = new Image
                    {
                        Width = 32,
                        Height = 32,
                        Source = new BitmapImage(
                            new Uri(
                                "C:\\Users\\amar-next\\Desktop\\Faks\\ParkingServis\\ParkingServis\\Images\\pLogo3.png"
                            )
                        ),
                        ToolTip = location.Name
                    }
                };

                // Add click event to the Image
                ((Image)marker.Shape).MouseLeftButtonUp += (sender, e) =>
                {
                    Border_Click(location);
                };

                // Add marker directly to the map
                MainMap.Markers.Add(marker);
            }
        }

        private async void GetLocations()
        {
            locations = await _locationQueryController.GetAllLocations();
        }

        public async void GenerateParkingLocationUI(List<Server.Models.Location> locations)
        {
            // Clear any existing column definitions and children
            locationsGrid.ColumnDefinitions.Clear();
            locationsGrid.Children.Clear();

            // Create column definitions dynamically based on the number of locations
            for (int i = 0; i < locations.Count; i++)
            {
                ColumnDefinition column = new ColumnDefinition
                {
                    Width = new GridLength(1, GridUnitType.Star) // or any desired width
                };
                locationsGrid.ColumnDefinitions.Add(column);
            }

            // Create UI elements dynamically for each location
            for (int i = 0; i < locations.Count; i++)
            {
                var location = locations[i];

                // Create Border
                Border border = new Border
                {
                    Background = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#3AA9AD")
                    ),
                    Margin = new Thickness(10),
                    CornerRadius = new CornerRadius(10),
                    Cursor = Cursors.Hand,
                    Name = $"border{location.Id}"
                };

                border.MouseLeftButtonUp += (s, e) => Border_Click(location);

                // Create StackPanel
                StackPanel stackPanel = new StackPanel
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Name = $"stackPanel{location.Id}"
                };

                // Create TextBlocks
                TextBlock nameTextBlock = new TextBlock
                {
                    Margin = new Thickness(0, 5, 0, 0),
                    Style = (Style)FindResource("titleText"),
                    FontSize = 18,
                    Text = $"Naziv: {location.Name}",
                    Name = "locationNameTb"
                };
                int activeSessions =
                    _parkingSessionQueryController.GetNumberOfActiveSessionsByLocationId(
                        location.Id
                    );
                TextBlock freeSpotsTextBlock = new TextBlock
                {
                    Name = $"numberOffFreeSpots{location.Id}",
                    Margin = new Thickness(0, 5, 0, 0),
                    Style = (Style)FindResource("titleText"),
                    FontSize = 18,
                    Text = $"Broj slobodnih mesta: {location.Capacity - activeSessions}"
                };

                // Add TextBlocks to StackPanel
                stackPanel.Children.Add(nameTextBlock);
                stackPanel.Children.Add(freeSpotsTextBlock);

                // Add StackPanel to Border
                border.Child = stackPanel;

                // Set Grid.Column property
                Grid.SetColumn(border, i);

                // Add Border to Grid
                locationsGrid.Children.Add(border);
            }

            //Change UI if parking session exists
            List<ParkingSession> existingSessions =
                await _parkingSessionQueryController.GetSessionsByUserId(Globals.CurrentUser.Id, 0);
            if (existingSessions.Count > 0)
            {
                foreach (var session in existingSessions)
                {
                    ChangeBorderColorOfParkedLocation(
                        session.LocationId,
                        session.VehicleReg,
                        session.ParkingStart,
                        session.Id
                    );
                }
            }
        }

        private void Border_Click(Server.Models.Location location)
        {
            Globals.ClickedLocation = location;
            if (Globals.CurrentUser.Role != "admin")
            {
                ParkDialog detailsWindow = _serviceProvider.GetRequiredService<ParkDialog>();
                detailsWindow.LocationUpdated += DetailsWindow_LocationUpdated;
                detailsWindow.Show();
            }
            else
            {
                LocationAdminDetails.locationId = location.Id;
                LocationAdminDetails.HomeWindow = this;
                LocationAdminDetails locationAdminDetails =
                    Client.ServiceProvider.serviceProvider.GetRequiredService<LocationAdminDetails>();
                locationAdminDetails.Show();
            }
        }

        private void DetailsWindow_LocationUpdated(object sender, EventArgs e)
        {
            if (!Globals.IsReservationSession)
            {
                ChangeBorderColorOfParkedLocation(
                    Globals.ClickedLocation.Id,
                    Globals.CurrentParkingSession.VehicleReg,
                    DateTime.Now,
                    Globals.CurrentParkingSession.Id
                );
            }
            else
            {
                ChangeBorderColorOfParkedLocation(
                    Globals.ClickedLocation.Id,
                    Globals.CurrentParkingSession.VehicleReg,
                    ReservationDate,
                    Globals.CurrentParkingSession.Id
                );
            }
            
        }

        private void ChangeBorderColorOfParkedLocation(
            int locationId,
            string regNumber,
            DateTime parkingStart,
            int sessionId
        )
        {
            foreach (var child in locationsGrid.Children)
            {
                if (child is Border border)
                {
                    if (border.Name == $"border{locationId}")
                    {
                        border.BorderThickness = new Thickness(3);
                        border.BorderBrush = !Globals.IsReservationSession
                            ? new SolidColorBrush(
                                (Color)ColorConverter.ConvertFromString("#AD3E3A")
                            )
                            : new SolidColorBrush(
                                (Color)ColorConverter.ConvertFromString("#FFD700")
                            );

                        if (border.Child is StackPanel panel)
                        {
                            if (panel.Name == $"stackPanel{locationId}")
                            {
                                foreach (var stackPchild in panel.Children)
                                {
                                    if (stackPchild is TextBlock textBlock)
                                    {
                                        int activeSessions =
                                            _parkingSessionQueryController.GetNumberOfActiveSessionsByLocationId(
                                                locationId
                                            );
                                        int capacity = locations
                                            .Where(l => l.Id == locationId)
                                            .FirstOrDefault()
                                            .Capacity;

                                        if (textBlock.Name == $"numberOffFreeSpots{locationId}")
                                        {
                                            textBlock.Text =
                                                $"Broj slobodnih mesta: {capacity - activeSessions}";
                                        }
                                    }
                                }
                                TextBlock nameTextBlock = new TextBlock
                                {
                                    Margin = new Thickness(0, 5, 0, 0),
                                    Style = (Style)FindResource("titleText"),
                                    FontSize = 18,
                                    Text = !Globals.IsReservationSession ? $"Parkirano vozilo: {regNumber}" : $"Rezervacija za vozilo: {regNumber}",
                                };
                                TextBlock timeTb = new TextBlock
                                {
                                    Margin = new Thickness(0, 5, 0, 0),
                                    Style = (Style)FindResource("titleText"),
                                    FontSize = 18,
                                    Text = $"Vreme pocetka parkinga :",
                                };

                                TextBlock time = new TextBlock
                                {
                                    Margin = new Thickness(0, 5, 0, 0),
                                    Style = (Style)FindResource("titleText"),
                                    FontSize = 18,
                                    Text = $"{parkingStart}",
                                };

                                Button endParkBtn = new Button
                                {
                                    Style = (Style)FindResource("button"),
                                    Background = Brushes.IndianRed,
                                    Foreground = Brushes.White,
                                    Margin = new Thickness(0, 15, 0, 0),
                                    Content = "Zavrsi parking"
                                };
                                endParkBtn.Click += (sender, e) =>
                                {
                                    string locationName = "";
                                    foreach (var element in panel.Children)
                                    {
                                        if (element is TextBlock tb)
                                        {
                                            if (tb.Name == "locationNameTb")
                                                locationName = tb.Text;
                                        }
                                    }

                                    ParkingPayment.RegNumber = regNumber;
                                    ParkingPayment.LocationName = locationName;
                                    ParkingPayment.parkingStart = parkingStart;
                                    ParkingPayment.sessionId = sessionId;

                                    ParkingPayment parkingPayment =
                                        _serviceProvider.GetRequiredService<ParkingPayment>();
                                    parkingPayment.ReloadHome += ParkingPaymentReloadHome;
                                    parkingPayment.Show();
                                };
                                panel.Children.Add(nameTextBlock);
                                panel.Children.Add(timeTb);
                                panel.Children.Add(time);
                                panel.Children.Add(endParkBtn);
                            }
                        }
                    }
                }
            }
        }

        public void ReloadHomeWindow()
        {
            HomeWindow home = _serviceProvider.GetRequiredService<HomeWindow>();
            home.Show();
            this.Close();
        }

        private void ParkingPaymentReloadHome(object sender, EventArgs e)
        {
            ReloadHomeWindow();
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LocationsWindow.HomeWindow = this;
            LocationsWindow locationsWindow =
                _serviceProvider.GetRequiredService<LocationsWindow>();
            locationsWindow.Show();
        }

        private void TransactionNav_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            TransactionsWindow transWindow =
                _serviceProvider.GetRequiredService<TransactionsWindow>();
            transWindow.Show();
        }

        private void profilNav_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UserProfilWindow window =
                _serviceProvider.GetRequiredService<UserProfilWindow>();
            window.Show();
        }

        private void TextBlock_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            VehiclesWindow window =
              _serviceProvider.GetRequiredService<VehiclesWindow>();
            window.Show();
        }

        private void logOutNav_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
              
            MainWindow window =
             _serviceProvider.GetRequiredService<MainWindow>();
            window.Show();
            Globals.CurrentUser = null;

            this.Close();
        }
    }
}
