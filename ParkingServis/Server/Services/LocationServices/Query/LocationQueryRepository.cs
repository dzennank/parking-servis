using MySqlConnector;
using ParkingServis.Server.Database;
using ParkingServis.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingServis.Server.Services.LocationServices.Query
{
    public class LocationQueryRepository : ILocationQueryRepository
    {
        public async Task<List<Location>> GetAllLocations()
        {
            Location location = new Location();
            List<Location> locationList = new List<Location>();
            try
            {
                DatabaseSettings connection = new DatabaseSettings();
                string sql = "SELECT * FROM `location`";
                using (MySqlDataReader reader = connection.executeQueryCommandWithoutParams(sql))
                {
                    while (reader.Read())
                    {
                        location = new Location
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("name"),
                            Address = reader.GetString("address"),
                            Capacity = reader.GetInt32("capacity"),
                            CoordinateX = reader.GetDouble("coordinateX"),
                            CoordinateY = reader.GetDouble("coordinateY")
                        };
                        locationList.Add(location);
                    }
                    
                }
                return locationList;
            }
            catch (Exception ex)
            {
                return locationList;
            }
        }
    }
}
