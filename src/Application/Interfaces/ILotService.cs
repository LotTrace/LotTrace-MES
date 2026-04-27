using LotTrace_MES.src.Application.DTO.Request.Lot;
using LotTrace_MES.src.Application.DTO.Response.Lot;
using LotTrace_MES.src.Domain.Enum;

namespace LotTrace_MES.src.Application.Interfaces
{
    public interface ILotService
    {
        Task<CreateResponseLotDTO?> CreateLotAsync(CreateRequestLotDTO createDTO); // Lot 생성
        Task<bool> MoveNextStepAsync(ChangeRequestLotDTO changeRequestDTO); // Lot의 다음 단계로 이동
        Task<bool> ChangeStateAsync(LotState newState, ChangeRequestLotDTO changeRequestDTO); // Lot 상태 변경
    }
}
