using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingServis.Server.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Capacity { get; set; }
        public double CoordinateX { get; set; }
        public double CoordinateY { get; set; }
    }
}
