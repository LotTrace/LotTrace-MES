using LotTrace_MES.src.Domain.Entity;
using LotTrace_MES.src.Domain.Interfaces.Common;

namespace LotTrace_MES.src.Domain.Interfaces.Services
{
    public interface IProductService : IService<Product>
    {
        Task<Product?> GetByProductCodeAsync(string productCode);
    }
}
