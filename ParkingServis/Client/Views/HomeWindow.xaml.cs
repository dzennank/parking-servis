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
        public HomeWindow(LocationQueryController locationQueryController, IServiceProvider serviceProvider, ParkingSessionQueryController parkingSessionQueryController)
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

        }

        private async void AddMarkerToLocations(List<Server.Models.Location> locations)
        {
            

            // Create a new marker
            foreach(Server.Models.Location location in locations)
            {
                PointLatLng position = new PointLatLng(location.CoordinateX, location.CoordinateY);
                GMapMarker marker = new GMapMarker(position)
                {
                    Shape = new Image
                    {
                        Width = 32,
                        Height = 32,
                        Source = new BitmapImage(new Uri("C:\\Users\\amar-next\\Desktop\\Faks\\ParkingServis\\ParkingServis\\Images\\pLogo3.png")),
                        ToolTip = location.Name
                    }
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
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3AA9AD")),
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
                    Text = $"Naziv: {location.Name}"
                };

                TextBlock freeSpotsTextBlock = new TextBlock
                {
                    Margin = new Thickness(0, 5, 0, 0),
                    Style = (Style)FindResource("titleText"),
                    FontSize = 18,
                    Text = $"Broj slobodnih mesta: {location.Capacity}"
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
            List<ParkingSession> existingSessions = await _parkingSessionQueryController.GetSessionsByUserId(Globals.CurrentUser.Id);
            if (existingSessions.Count > 0)
            {
                foreach (var session in existingSessions)
                {
                    ChangeBorderColorOfParkedLocation(session.LocationId);

                }
            }
        }

        private void Border_Click(Server.Models.Location location)
        {
            Globals.ClickedLocation = location;
            ParkDialog detailsWindow = _serviceProvider.GetRequiredService<ParkDialog>();
            detailsWindow.LocationUpdated += DetailsWindow_LocationUpdated;
            detailsWindow.Show();
        }
        private void DetailsWindow_LocationUpdated(object sender, EventArgs e)
        {
            ChangeBorderColorOfParkedLocation(Globals.ClickedLocation.Id);
        }
        private void ChangeBorderColorOfParkedLocation(int locationId)
        {
            foreach(var child in locationsGrid.Children)
            {
                if(child is Border border)
                {
                    if(border.Name == $"border{locationId}")
                    {
                        border.BorderThickness = new Thickness(3);
                        border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AD3E3A"));

                        if(border.Child is StackPanel panel)
                        {
                            if (panel.Name == $"stackPanel{locationId}")
                            {
                                TextBlock nameTextBlock = new TextBlock
                                {
                                    Margin = new Thickness(0, 5, 0, 0),
                                    Style = (Style)FindResource("titleText"),
                                    FontSize = 18,
                                    Text = $"Parkirano vozilo: NP1312PP",

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
                                    Text = $"{DateTime.Now}",

                                };

                                panel.Children.Add(nameTextBlock);
                                panel.Children.Add(timeTb);
                                panel.Children.Add(time);
                            }
                        }
                    }
                   
                }
              
            }
        }

    }
}
