using ParkingServis.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingServis.Server.Services.ParkingSessionServices.Query
{
    public interface IParkingSessionQueryRepository
    {
        Task<List<ParkingSession>> GetParkingSessionByUserId(int userId, int paymentStaus);
        int GetLastId();
        Task<List<ParkingSession>> GetParkingSessionByLocationId(int locationId);

        int GetNumberOfActiveSessionsByLocationId(int locationId);
    }
}
