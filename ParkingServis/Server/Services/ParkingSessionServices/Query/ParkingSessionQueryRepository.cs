using MigraDoc.DocumentObjectModel;
using MySqlConnector;
using ParkingServis.Server.Database;
using ParkingServis.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ParkingServis.Server.Services.ParkingSessionServices.Query
{
    public class ParkingSessionQueryRepository : IParkingSessionQueryRepository
    {
        public int GetLastId()
        {
            DatabaseSettings connection = new DatabaseSettings();
            int id = 0;
            string sql = "SELECT id FROM parking_session ORDER BY id DESC LIMIT 1;";
            using (MySqlDataReader reader = connection.executeQueryCommandWithoutParams(sql))
            {
                if (reader.Read())
                {
                    id = reader.GetInt32("id"); 
                }

            }
            return id;

        }

        public async Task<List<ParkingSession>> GetParkingSessionByLocationId(int locationId)
        {
            List<ParkingSession> sessions = new List<ParkingSession>();
            try
            {
                DatabaseSettings connection = new DatabaseSettings();
                string sql = "SELECT * FROM `parking_session` WHERE `location_id` = @locationId AND `payment_status` = @paymentStatus";
                var parametars = new Dictionary<string, object>
                {
                    { "@locationId", locationId },
                    { "@paymentStatus", 0 }
                };
                using (MySqlDataReader reader = await connection.executeQueryCommandAsyncParams(sql, parametars))
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

        public int GetNumberOfActiveSessionsByLocationId(int locationId)
        {
            int numberOfSessions = 0;
            try
            {
                DatabaseSettings connection = new DatabaseSettings();
                string sql = "SELECT COUNT(*) FROM `parking_session` WHERE `payment_status` = @paymentStatus";
                var parameters = new Dictionary<string, object>
                {
                    { "@paymentStatus", 0 }
                };
                using (MySqlDataReader reader = connection.executeQueryCommand(sql, parameters))
                {
                    if (reader.Read())
                    {
                        numberOfSessions = reader.GetInt32(0);
                    }

                }

                return numberOfSessions;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public async Task<List<ParkingSession>> GetParkingSessionByUserId(int userId, int paymentStatus)
        {
                List<ParkingSession> sessions = new List<ParkingSession>();
            try
            {
                DatabaseSettings connection = new DatabaseSettings();
                string sql = "SELECT * FROM `parking_session` WHERE `user_id` = @userId AND `payment_status` = @paymentStatus";
                var parametars = new Dictionary<string, object>
                {
                    { "@userId", userId },
                    { "@paymentStatus", paymentStatus }
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
