using LotTrace_MES.src.Domain.Entity;
using LotTrace_MES.src.Domain.Interfaces;
using LotTrace_MES.src.Infrastructure.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace LotTrace_MES.src.Infrastructure.Persistence.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Order>> GetOrdersWithProductAsync()
        {
            return await _dbSet.Include(o => o.Product).OrderByDescending(o => o.OrderId).ToListAsync();
        }
    }
}
