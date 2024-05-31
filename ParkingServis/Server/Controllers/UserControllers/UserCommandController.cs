using ParkingServis.Server.Models;
using ParkingServis.Server.Services.UserServices.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingServis.Server.Controllers.UserControllers
{
    public class UserCommandController
    {
        private readonly UserCommandInterface _commandRepository;
        public UserCommandController(UserCommandInterface commandRepository)
        {
            _commandRepository = commandRepository;
        }

        public async Task<bool> CreateUser(User user)
        {
            try
            {
                await _commandRepository.CreateUser(user);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
