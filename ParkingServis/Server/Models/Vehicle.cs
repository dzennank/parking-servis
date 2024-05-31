using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingServis.Server.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string RegNumber { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public int ProductionYear { get; set; }
    }
}
