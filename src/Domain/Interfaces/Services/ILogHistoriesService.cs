using LotTrace_MES.src.Domain.Entity;
using LotTrace_MES.src.Domain.Interfaces.Common;

namespace LotTrace_MES.src.Domain.Interfaces.Services
{
    public interface ILogHistoriesService : IService<LogHistories>
    {
        Task<IEnumerable<LogHistories>> GetHistoryByLotIdAsync(int lotId);
        Task<IEnumerable<LogHistories>> GetByDateAsync(DateTime start, DateTime end);
    }
}
