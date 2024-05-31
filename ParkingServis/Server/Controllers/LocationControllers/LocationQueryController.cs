using ParkingServis.Server.Models;
using ParkingServis.Server.Services.LocationServices.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingServis.Server.Controllers.LocationControllers
{
    public class LocationQueryController
    {
        private readonly ILocationQueryRepository _repository;
        public LocationQueryController(ILocationQueryRepository queryRepository)
        {
            _repository = queryRepository;
        }

        public async Task<List<Location>> GetAllLocations()
        {
            return await _repository.GetAllLocations();
        }
    }
}
