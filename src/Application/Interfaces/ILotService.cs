using LotTrace_MES.src.Application.DTO.Request.Lot;
using LotTrace_MES.src.Application.DTO.Response.Lot;
using LotTrace_MES.src.Domain.Enum;

namespace LotTrace_MES.src.Application.Interfaces
{
    public interface ILotService
    {
        Task<ResponseLotDTO?> CreateLotAsync(CreateRequestLotDTO createDTO); // Lot 생성
        Task<bool> MoveNextStepAsync(ChangeRequestLotDTO changeRequestDTO); // Lot의 다음 단계로 이동
        Task<bool> ChangeStateAsync(LotState newState, ChangeRequestLotDTO changeRequestDTO); // Lot 상태 변경
        Task<IEnumerable<ResponseLotDTO>> GetLotsAsync(); // 모든 Lot 조회
        Task<ResponseLotDTO?> GetLotByIdAsync(int lotId); // Lot ID로 조회
        Task<ResponseLotDTO?> GetLotByBarcodeAsync(string barcode); // Lot Barcode로 조회
        Task<IEnumerable<ResponseLotDTO>> GetLotsByLineIdAsync(int lineId); // 라인 ID로 조회
        Task<bool> DeleteLotAsync(int lotId); // Lot 삭제
        Task<IEnumerable<ResponseLotDTO>> GetLotsByStateAsync(LotState state); // Lot 상태로 조회
    }
}
