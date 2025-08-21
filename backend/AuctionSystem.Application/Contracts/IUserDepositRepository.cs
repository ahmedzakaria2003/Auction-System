using AuctionSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.Contracts
{
    public interface IUserDepositRepository:IGenericRepository<UserDeposite>
    {
        

        Task<UserDeposite?> GetByPaymentIntentIdAsync(string intentId);

        Task<bool> HasPaidDepositeAsync(Guid userId, Guid auctionId);

    }
}
