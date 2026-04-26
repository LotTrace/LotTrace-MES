using LotTrace_MES.src.Domain.Entity;
using LotTrace_MES.src.Domain.Enum;

namespace LotTrace_MES.src.Application.Interfaces
{
    public interface ILotService
    {
        Task<Lot?> CreateLotAsync(string barcode, int productId, int workerId, int lineId); // Lot 생성
        Task<bool> MoveNextStepAsync(string barcode, int workerId); // Lot의 다음 단계로 이동
        Task<bool> ChangeStateAsync(string barcode, LotState newState, int workerId); // Lot 상태 변경
    }
}
