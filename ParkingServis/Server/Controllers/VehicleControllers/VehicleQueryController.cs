using ParkingServis.Server.Models;
using ParkingServis.Server.Services.VehicleServices.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingServis.Server.Controllers.VehicleControllers
{
   public class VehicleQueryController
    {
        private readonly IVehicleQueryRepository _vehicleQueryRepository;
        public VehicleQueryController(IVehicleQueryRepository vehicleQueryRepository)
        {
            _vehicleQueryRepository = vehicleQueryRepository;
        }

        public List<Vehicle> GetVehiclesByUserId(int userId)
        {
            return _vehicleQueryRepository.GetVehiclesByUserId(userId);
        }
    }
}
