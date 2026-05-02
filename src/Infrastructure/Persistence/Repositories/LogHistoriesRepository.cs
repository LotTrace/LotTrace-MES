using LotTrace_MES.src.Domain.Entity;
using LotTrace_MES.src.Domain.Interfaces;
using LotTrace_MES.src.Infrastructure.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace LotTrace_MES.src.Infrastructure.Persistence.Repositories
{
    public class LogHistoriesRepository : GenericRepository<LogHistories>, ILogHistoriesRepository // 상속을 받았기 떄문에 _context를 다시 정의할 필요가 없다.
    {
        public LogHistoriesRepository(AppDbContext context) : base(context) // 상속받은 부모 파일에 context를 넘겨준다.
        {

        }

        private IQueryable<LogHistories> DefaultQuery => _dbSet
            .Include(lh => lh.Worker)
            .OrderByDescending(lh => lh.EventTime)
            .AsNoTracking();

        public async Task<IEnumerable<LogHistories>> GetByDateAsync(DateTime start, DateTime end) // 날짜 범위로 로그 조회
        {
            return await DefaultQuery
                .Where(lh => lh.EventTime >= start && lh.EventTime <= end)
                .ToListAsync();
        }

        public async Task<IEnumerable<LogHistories>> GetHistoryByLotIdAsync(int lotId) // LotId로 로그 조회
        {
            return await DefaultQuery
                .Where(lh => lh.LotId == lotId)
                .ToListAsync();
        }

        public override async Task<IEnumerable<LogHistories>> GetAllAsync()
        {
            return await DefaultQuery.ToListAsync();
        }
    }
}
