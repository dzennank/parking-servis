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

        public async Task<bool> UpdateUser(User user)
        {
            return await _commandRepository.UpdateUser(user);
        }

        public async Task<bool> UpdateUserCreditsAfterPayment(decimal credits, int userId)
        {
            return await _commandRepository.UpdateUserCreditsAfterPayment(credits, userId);
        }

        public async Task<bool> UpdateUserCredits(decimal credits, int userId)
        {
            return await _commandRepository.UpdateUserCredits(credits, userId);
        }
    }
}
