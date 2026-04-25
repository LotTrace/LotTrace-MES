using LotTrace_MES.Domain.Interfaces;
using LotTrace_MES.src.Domain.Entity;
using LotTrace_MES.src.Domain.Interfaces.Common;
using LotTrace_MES.src.Infrastructure.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace LotTrace_MES.src.Infrastructure.Persistence.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context) {
            }

        public async Task<Product?> GetProductCodeAsync(string productCode)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.ProductCode == productCode);
        }
    }
}
