using LotTrace_MES.src.Domain.Entity;

namespace LotTrace_MES.src.Domain.Interfaces
{
    public interface IMaterialRepository
    {
        Task<IEnumerable<Material>> GetLowStockMaterialAsync(int threshold = 10);
    }
}
