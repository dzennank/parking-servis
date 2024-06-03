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
                string sql =
                    "INSERT into `users` (first_name, last_name, password, email, adress, credits) VALUES (@firstName, @lastName, @password, @email, @adress, @credits)";
                var parameters = new Dictionary<string, object>
                {
                    { "@firstName", user.FirstName },
                    { "@lastName", user.LastName },
                    { "@password", user.Password },
                    { "@email", user.Email },
                    { "@adress", user.Adress },
                    { "@credits", user.Credits }
                };
                await connection.executeQueryCommandAsyncParams(sql, parameters);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateUserCredits(decimal credits, int userId)
        {
            try
            {
                DatabaseSettings connection = new DatabaseSettings();
                string sql = "UPDATE `users` SET `credits` = credits - @credits WHERE `id` = @userId";
                var parametars = new Dictionary<string, object>
                {
                    { "@credits", credits },
                    { "userId", userId },
                };
                await connection.executeQueryCommandAsyncParams(sql, parametars);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
