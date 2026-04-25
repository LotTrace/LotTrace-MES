using LotTrace_MES.src.Domain.Entity;
using LotTrace_MES.src.Domain.Interfaces;
using LotTrace_MES.src.Infrastructure.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace LotTrace_MES.src.Infrastructure.Persistence.Repositories
{
    public class LogHistoriesRepository : GenericRepository<LogHistories>, ILogHistoriesRepository // 상속을 받았기 떄문에 _context를 다시 정의할 필요가 없다.
    {
        public LogHistoriesRepository(AppDbContext context) : base(context) // 상속받은 부모 파일에 context를 넘겨준다.
        {

        }

        public async Task<IEnumerable<LogHistories>> GetByDateAsync(DateTime start, DateTime end) // 날짜를 기준으로 로그 기록 조회
        {
            return await _dbSet.Where(lh => lh.EventTime >= start && lh.EventTime <= end).ToListAsync();
        }

        public async Task<IEnumerable<LogHistories>> GetHistoryByLotIdAsync(int lotId) // LotId 기준으로 로그 조회
        {
            return await _dbSet.Where(lh => lh.LotId == lotId)
                                .OrderByDescending(lh => lh.EventTime)
                                .ToListAsync();
        }
    }
}
