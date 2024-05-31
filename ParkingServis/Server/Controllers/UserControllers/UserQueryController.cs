using ParkingServis.Server.Models;
using ParkingServis.Server.Services.UserServices.Command;
using ParkingServis.Server.Services.UserServices.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingServis.Server.Controllers.UserControllers
{
    public class UserQueryController
    {
        private readonly UserQueryInterface _queryRepository;
        public UserQueryController(UserQueryInterface queryRepository)
        {
            _queryRepository = queryRepository;
        }

        public async Task<User> GetUserByEmailPassword(string email, string password)
        {

            return await _queryRepository.GetUserByEmailPassword(email, password);
        }
    }
}
