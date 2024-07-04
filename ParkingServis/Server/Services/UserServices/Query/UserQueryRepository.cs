using MySqlConnector;
using ParkingServis.Server.Database;
using ParkingServis.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ParkingServis.Server.Services.UserServices.Query
{
    public class UserQueryRepository : UserQueryInterface
    {
        public async Task<User> GetUserByEmailPassword(string email, string password)
        {
            User user = new User();
            try
            {
                DatabaseSettings connection = new DatabaseSettings();
               
                string sql = "SELECT * FROM `users` WHERE email = @email AND `password` = @password ";
                var parameters = new Dictionary<string, object>
            {
                {"@email", email },
                {"@password", password}               
               
            };
                using(MySqlDataReader reader = connection.executeQueryCommand(sql, parameters))
                {
                    if (reader.Read())
                    {
                        user = new User
                        {
                            Id = reader.GetInt32("id"),
                            FirstName = reader.GetString("first_name"),
                            LastName = reader.GetString("last_name"),
                            Email = reader.GetString("email"),
                            Password = reader.GetString("password"),
                            Adress = reader.GetString("adress"),
                            Credits = reader.GetDecimal("credits"),
                            Role = reader.GetString("role")
                        };
                    }
                }
                return user;
               
            }
            catch (Exception ex)
            {
                return user;
                Console.WriteLine(ex.Message);
            }
        }
    }
}
