using LotTrace_MES.Domain.Interfaces;
using LotTrace_MES.src.Domain.Entity;
using LotTrace_MES.src.Domain.Interfaces.Common;
using LotTrace_MES.src.Infrastructure.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace LotTrace_MES.src.Infrastructure.Persistence.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository // 상속을 받았기 떄문에 _context를 다시 정의할 필요가 없다.
    {
        public ProductRepository(AppDbContext context) : base(context)
        { // 상속받은 부모 파일에 context를 넘겨준다.
        }

        public async Task<Product?> GetProductCodeAsync(string productCode) // 제품코드로 제품 조회
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.ProductCode == productCode);
        }
    }
}
