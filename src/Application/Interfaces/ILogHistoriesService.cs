using LotTrace_MES.src.Application.DTO.Response.LogHistories;

namespace LotTrace_MES.src.Application.Interfaces
{
    public interface ILogHistoriesService
    {
        Task<IEnumerable<ResponseLogDTO>> GetLogHistoriesByLotIdAsync(int lotId); // LotId로 검색
        Task<IEnumerable<ResponseLogDTO>> GetLogHistoriesByBarcodeAsync(string barcode); // 바코드 조회
        Task<IEnumerable<ResponseLogDTO>> GetLogHistoriesByDateAsync(DateTime start, DateTime end); // 날짜 조회
    }
}
