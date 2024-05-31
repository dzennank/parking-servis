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
        Task<List<ParkingSession>> GetParkingSessionByUserId(int userId);
    }
}
