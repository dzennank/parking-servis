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
    }
}
