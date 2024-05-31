using ParkingServis.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingServis.Server.Services.UserServices.Query
{
    public interface UserQueryInterface
    {
        public Task<User> GetUserByEmailPassword(string email, string password);

    }
}
