using ParkingServis.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingServis.Server.Services.LocationServices.Query
{
    public interface ILocationQueryRepository
    {
        public Task<List<Location>> GetAllLocations();
    }
}
