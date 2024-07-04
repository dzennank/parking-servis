using ParkingServis.Server.Database;
using ParkingServis.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingServis.Server.Services.VehicleServices.Command
{
    public class VehicleCommandRepository : IVehicleCommandRepository
    {
        public async Task<bool> CreateVehicle(Vehicle vehicle, int userId)
        {
            try
            {
                DatabaseSettings connection = new DatabaseSettings();
                string sql =
                    "INSERT into `vehicle` (user_id, reg_number, brand, Model, Color, production_year) VALUES (@userId, @regNumber, @brand, @model, @color, @prodYear)";
                var parameters = new Dictionary<string, object>
                {
                    { "@userId", userId },
                    { "@regNumber", vehicle.RegNumber },
                    { "@brand", vehicle.Brand },
                    { "@model", vehicle.Model },
                    { "@color", vehicle.Color },
                    { "@prodYear", vehicle.ProductionYear }
                };
                await connection.executeQueryCommandAsyncParams(sql, parameters);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
