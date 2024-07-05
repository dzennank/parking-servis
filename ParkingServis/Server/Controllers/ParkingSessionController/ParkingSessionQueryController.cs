using ParkingServis.Server.Models;
using ParkingServis.Server.Services.ParkingSessionServices.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingServis.Server.Controllers.ParkingSessionController
{
    public class ParkingSessionQueryController
    {
        private readonly IParkingSessionQueryRepository _parkingSessionQueryRepository;
        public ParkingSessionQueryController(IParkingSessionQueryRepository parkingSessionQueryRepository)
        {
            _parkingSessionQueryRepository = parkingSessionQueryRepository;
        }

        public async Task<List<ParkingSession>> GetSessionsByUserId(int userId, int paymentStatus)
        {
            return await _parkingSessionQueryRepository.GetParkingSessionByUserId(userId, paymentStatus);
        }
        public int GetLastId()
        {
            return _parkingSessionQueryRepository.GetLastId();
        }

        public async Task<List<ParkingSession>> GetSessionsByLocationId(int locationId)
        {
            return await _parkingSessionQueryRepository.GetParkingSessionByLocationId(locationId);
        }

        public int GetNumberOfActiveSessionsByLocationId(int locationId)
        {
            return _parkingSessionQueryRepository.GetNumberOfActiveSessionsByLocationId(locationId);
        }
        public List<ParkingSession> GetParkingSessions()
        {
            return _parkingSessionQueryRepository.GetParkingSessions();
        }
    }
}
