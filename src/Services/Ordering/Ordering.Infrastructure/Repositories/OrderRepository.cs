using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persistance;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        protected readonly OrderContext _dbContext;

        public OrderRepository(OrderContext dbContext) 
                            : base(dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName)
        {
           var orderList = await _dbContext.Orders.Where(x => x.UserName == userName)
                                                   .ToListAsync();
            return orderList;
        }
    }
}
