﻿using ParkingServis.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingServis.Server.Services.UserServices.Command
{
    public interface UserCommandInterface
    {
        public Task<bool> CreateUser(User user);
    }
}
