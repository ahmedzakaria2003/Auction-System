﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.Contracts
{
   public interface IDataSeeding
    {
        Task SeedDataAsync();
    }
}
