using MySqlConnector;
using ParkingServis.Server.Database;
using ParkingServis.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingServis.Server.Services.ParkingSessionServices.Query
{
    public class ParkingSessionQueryRepository : IParkingSessionQueryRepository
    {
        public async Task<List<ParkingSession>> GetParkingSessionByUserId(int userId)
        {
                List<ParkingSession> sessions = new List<ParkingSession>();
            try
            {
                DatabaseSettings connection = new DatabaseSettings();
                string sql = "SELECT * FROM `parking_session` WHERE `user_id` = @userId AND `payment_status` = @paymentStatus";
                var parametars = new Dictionary<string, object>
                {
                    { "@userId", userId },
                    { "@paymentStatus", 0 }
                };
                using(MySqlDataReader reader = await connection.executeQueryCommandAsyncParams(sql, parametars))
                {
                    while (reader.Read())
                    {
                        ParkingSession session = new ParkingSession
                        {
                            Id = reader.GetInt32("id"),
                            UserId = reader.GetInt32("user_id"),
                            LocationId = reader.GetInt32("location_id"),
                            VehicleReg = reader.GetString("vehicle_id"),
                            ParkingStart = reader.GetDateTime("parking_start"),
                            ParkingEnd = reader.GetDateTime("parking_end"),
                            PaymentStatus = reader.GetBoolean("payment_status"),
                            PricePaid = reader.GetDecimal("price_paid")
                        };
                        sessions.Add(session);
                    }
                    
                }
                return sessions;
            }
            catch (Exception ex)
            {
                return sessions;
            }
        }
    }
}
