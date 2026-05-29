using LotTrace_MES.src.Domain.Entity;
using LotTrace_MES.src.Infrastructure.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace LotTrace_MES.src.Infrastructure.Persistence.Repositories
{
    public class MaterialRepository : GenericRepository<Material>
    {
        public MaterialRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Material>> GetLowStockMaterialAsync(int threshold = 10)
        {
            return await _dbSet.Where(m => m.Stock <= threshold).OrderBy(m => m.Stock).ToListAsync();
        }
    }
}
