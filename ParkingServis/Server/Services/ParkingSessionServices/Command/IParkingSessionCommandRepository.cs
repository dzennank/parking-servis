using ParkingServis.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingServis.Server.Services.ParkingSessionServices.Command
{
    public interface IParkingSessionCommandRepository
    {
       Task<bool> AddParkingSession(ParkingSession parkingSession);
        Task<bool> PayParkingSession(int sessionId, decimal price, DateTime parkEnd);
    }
}
