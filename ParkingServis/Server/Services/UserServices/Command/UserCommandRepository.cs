using ParkingServis.Server.Database;
using ParkingServis.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingServis.Server.Services.UserServices.Command
{
    public class UserCommandRepository : UserCommandInterface
    {
        public async Task<bool> CreateUser(User user)
        {
            try
            {
                DatabaseSettings connection = new DatabaseSettings();
                string sql = "INSERT into `users` (first_name, last_name, password, email, adress) VALUES (@firstName, @lastName, @password, @email, @adress)";
                var parameters = new Dictionary<string, object>
            {
                {"@firstName", user.FirstName },
                {"@lastName", user.LastName},
                {"@password", user.Password },
                {"@email", user.Email },
                {"@adress", user.Adress }
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
