using ParkingServis.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingServis.Client
{
    public class Globals
    {
        public static User CurrentUser { get; set; }
        public static Location ClickedLocation { get; set; }
        public static string ParkedVehicleRegNumber { get; set; }
        public static ParkingSession CurrentParkingSession { get; set; }

        public static bool IsReservationSession { get; set; }
    }
}
