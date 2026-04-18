using LotTrace_MES.src.Domain.Entity;
using LotTrace_MES.src.Domain.Interfaces.Common;

namespace LotTrace_MES.src.Domain.Interfaces
{
    public interface ILogHistoriesRepository : IRepository<LogHistories>
    {
        Task<IEnumerable<LogHistories>> GetHistoryByLotIdAsync(int lotId);
        Task<IEnumerable<LogHistories>> GetByDateAsync(DateTime start, DateTime end);
    }
}
