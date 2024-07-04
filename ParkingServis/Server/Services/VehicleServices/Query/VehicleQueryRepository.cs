using GMap.NET.MapProviders;
using MySqlConnector;
using ParkingServis.Server.Database;
using ParkingServis.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingServis.Server.Services.VehicleServices.Query
{
    public class VehicleQueryRepository : IVehicleQueryRepository
    {
        public List<Vehicle> GetVehiclesByUserId(int userId)
        {
            List<Vehicle> vehicles = new List<Vehicle>();
            DatabaseSettings connection = new DatabaseSettings();
            string sql = $"SELECT * FROM vehicle WHERE user_id = @userId";

            var parameters = new Dictionary<string, object>
            {
                {"@userId", userId }

            };

            using(MySqlDataReader reader = connection.executeQueryCommand(sql, parameters))
            {
                while (reader.Read())
                {
                    Vehicle vehicle = new Vehicle
                    {
                        Id = reader.GetInt32("id"),
                        RegNumber = reader.GetString("reg_number"),
                        Brand = reader.GetString("brand"),
                        Model = reader.GetString("Model"),
                        Color = reader.GetString("Color"),
                        ProductionYear = reader.GetInt32("production_year")
                    };
                    vehicles.Add(vehicle);
                }
            }
            return vehicles;
        }
    }
}
