using LotTrace_MES.Domain.Interfaces;
using LotTrace_MES.src.Domain.Entity;
using LotTrace_MES.src.Domain.Enum;
using LotTrace_MES.src.Infrastructure.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace LotTrace_MES.src.Infrastructure.Persistence.Repositories
{
    public class LotRepository : GenericRepository<Lot>, ILotRepository // 상속을 받았기 떄문에 _context를 다시 정의할 필요가 없다.
    {

        public LotRepository(AppDbContext context) : base(context) // 상속받은 부모 파일에 context를 넘겨준다.
        {
        }

        public async Task<Lot?> GetByBarcodeAsync(string barcode) // 바코드번호로 Lot 조회
        {
            return await _dbSet.FirstOrDefaultAsync(l => l.Barcode == barcode); // FirstOrDefaultAsync는 조건에 맞는 첫 번째 요소를 반환하거나, 그런 요소가 없으면 null을 반환한다.
        }

        public async Task<IEnumerable<Lot>> GetByLineIdAsync(int lineId) // 해당 라인에서 생산된 Lot 조회
        {
            return await _dbSet.Where(l => l.LineId == lineId).ToListAsync();
        }

        public async Task<IEnumerable<Lot>> GetByStateAsync(LotState state) // 해당 상태에 있는 Lot 조회
        {
            return await _dbSet.Where(l => l.CurrentState == state).ToListAsync();
        }
    }
}
