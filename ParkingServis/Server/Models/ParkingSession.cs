using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingServis.Server.Models
{
    public class ParkingSession
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int LocationId { get; set; }
        public string VehicleReg { get; set; }
        public DateTime ParkingStart { get; set; }
        public DateTime ParkingEnd { get; set; }
        public bool PaymentStatus { get; set; }
        public decimal PricePaid { get; set; }
    }
}
