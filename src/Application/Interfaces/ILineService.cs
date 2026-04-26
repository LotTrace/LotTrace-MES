using LotTrace_MES.src.Domain.Entity;
using LotTrace_MES.src.Domain.Enum;

namespace LotTrace_MES.src.Application.Interfaces
{
    public interface ILineService
    {
        Task<bool> UpdateLineStateAsync(int lineId, LineState newState, int workerId); // 특정 라인의 상태를 변경
        Task<IEnumerable<Line>> GetByLineStateAsync(LineState state); // 라인 상태에 따른 조회
        Task<Line?> GetByLineIdAsync(int lineId); // 라인 아이디로 현재 상태 조회
    }
}
