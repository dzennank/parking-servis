using ParkingServis.Server.Database;
using ParkingServis.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingServis.Server.Services.ParkingSessionServices.Command
{
    public class ParkingSessionCommandRepository : IParkingSessionCommandRepository
    {
        public async Task<bool> AddParkingSession(
            ParkingSession parkingSession
        )
        {
            try
            {
                DatabaseSettings connection = new DatabaseSettings();
                string sql =
                    "INSERT INTO `parking_session` (`user_id`, `location_id`, `vehicle_id`, `parking_start`, `parking_end`, `payment_status`, `price_paid`) " +
                    "VALUES(@userId, @locationId, @vehicleId, @parkingStart, @parkingEnd, @paymentStatus, @pricePaid)";

                var parametars = new Dictionary<string, object>
                {
                    {"@userId", parkingSession.UserId },
                    {"@locationId", parkingSession.LocationId },
                    {"@vehicleId", parkingSession.VehicleReg },
                    {"@parkingStart", parkingSession.ParkingStart},
                    {"@parkingEnd", parkingSession.ParkingEnd},
                    {"@paymentStatus", parkingSession.PaymentStatus},
                    {"@pricePaid", parkingSession.PricePaid},
                };

                await connection.executeQueryCommandAsyncParams(sql, parametars);
                return true;
            }
            catch (Exception ex) 
            {
                return false;
            }
            
        }
    }
}
