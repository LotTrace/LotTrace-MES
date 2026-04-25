using LotTrace_MES.src.Domain.Entity;
using LotTrace_MES.src.Domain.Interfaces;
using LotTrace_MES.src.Infrastructure.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace LotTrace_MES.src.Infrastructure.Persistence.Repositories
{
    public class LogHistoriesRepository : GenericRepository<LogHistories>, ILogHistoriesRepository
    {
        public LogHistoriesRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<LogHistories>> GetByDateAsync(DateTime start, DateTime end)
        {
            return await _dbSet.Where(lh => lh.EventTime >= start && lh.EventTime <= end).ToListAsync();
        }

        public async Task<IEnumerable<LogHistories>> GetHistoryByLotIdAsync(int lotId)
        {
            return await _dbSet.Where(lh => lh.LotId == lotId).ToListAsync();
        }
    }
}
