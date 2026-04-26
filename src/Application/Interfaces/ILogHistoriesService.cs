using LotTrace_MES.src.Domain.Entity;

namespace LotTrace_MES.src.Application.Interfaces
{
    public interface ILogHistoriesService
    {
        Task<IEnumerable<LogHistories>> GetLogHistoriesByIdAsync(int lotId); // LotId로 검색
        Task<IEnumerable<LogHistories>> GetLogHistoriesByBarcodeAsync(int lotId); // 바코드 조회
        Task<IEnumerable<LogHistories>> GetLogHistoriesByDateAsync(DateTime start, DateTime end); // 날짜 조회
    }
}
