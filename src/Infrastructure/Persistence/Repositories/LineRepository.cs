using LotTrace_MES.src.Domain.Entity;
using LotTrace_MES.src.Domain.Enum;
using LotTrace_MES.src.Domain.Interfaces;
using LotTrace_MES.src.Infrastructure.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace LotTrace_MES.src.Infrastructure.Persistence.Repositories
{
    public class LineRepository : GenericRepository<Line>, ILineRepository // 상속을 받았기 떄문에 _context를 다시 정의할 필요가 없다.
    {
        public LineRepository(AppDbContext context) : base(context) // 상속받은 부모 파일에 context를 넘겨준다.
        {

        }
        public async Task<IEnumerable<Line>> GetByLineState(LineState state) // On, Off 상태에 따른 라인 조회
        {
            return await _dbSet.Where(l => l.CurrentState == state).ToListAsync();
        }
    }
}
