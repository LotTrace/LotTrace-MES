using LotTrace_MES.src.Domain.Entity;
using LotTrace_MES.src.Domain.Interfaces.Common;

namespace LotTrace_MES.src.Domain.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersWithProductAsync();
    }
}
