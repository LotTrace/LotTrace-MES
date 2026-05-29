using LotTrace_MES.src.Domain.Entity;
using LotTrace_MES.src.Domain.Interfaces.Common;

namespace LotTrace_MES.src.Domain.Interfaces
{
    public interface IMaterialRepository : IRepository<Material>
    {
        Task<IEnumerable<Material>> GetLowStockMaterialAsync(int threshold = 10);
    }
}
