using ParkingServis.Server.Models;
using ParkingServis.Server.Services.VehicleServices.Command;
using ParkingServis.Server.Services.VehicleServices.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingServis.Server.Controllers.VehicleControllers
{
    public class VehicleCommandController
    {
        private readonly IVehicleCommandRepository _vehicleCommandRepository;

        public VehicleCommandController(IVehicleCommandRepository vehicleCommandRepository)
        {
            _vehicleCommandRepository = vehicleCommandRepository;
        }
        public async Task<bool> CreateVehicle(Vehicle vehicle, int userId)
        {
            return await _vehicleCommandRepository.CreateVehicle(vehicle, userId);
        }
    }
}
