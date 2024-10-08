﻿using ParkingServis.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingServis.Server.Services.VehicleServices.Command
{
    public interface IVehicleCommandRepository
    {
        public Task<bool> CreateVehicle(Vehicle vehicle, int userId);
    }
}
