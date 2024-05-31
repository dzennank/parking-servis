using ParkingServis.Server.Models;
using ParkingServis.Server.Services.ParkingSessionServices.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingServis.Server.Controllers.ParkingSessionController
{
    public class ParkingcSessionCommandController
    {
        private readonly IParkingSessionCommandRepository _parkingSessionCommandRepository;
        public ParkingcSessionCommandController(IParkingSessionCommandRepository parkingSessionCommandRepository)
        {
            _parkingSessionCommandRepository = parkingSessionCommandRepository;
        }

        public async Task<bool> AddParkingSession(ParkingSession parkingSession)
        {           
            return await _parkingSessionCommandRepository.AddParkingSession(parkingSession);
           
        }
    }
}
