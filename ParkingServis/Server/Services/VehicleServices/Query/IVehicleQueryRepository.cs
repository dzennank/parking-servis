using ParkingServis.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingServis.Server.Services.VehicleServices.Query
{
    public interface IVehicleQueryRepository
    {
        public List<Vehicle> GetVehiclesByUserId(int userId);
    }
}
