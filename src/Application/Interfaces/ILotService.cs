using LotTrace_MES.src.Application.DTO.Request.Lot;
using LotTrace_MES.src.Application.DTO.Response.Lot;
using LotTrace_MES.src.Domain.Entity;
using LotTrace_MES.src.Domain.Enum;

namespace LotTrace_MES.src.Application.Interfaces
{
    public interface ILotService
    {
        Task<CreateResponseDTO?> CreateLotAsync(CreateRequestDTO createDTO); // Lot 생성
        Task<bool> MoveNextStepAsync(ChangeRequestDTO changeRequestDTO); // Lot의 다음 단계로 이동
        Task<bool> ChangeStateAsync(LotState newState, ChangeRequestDTO changeRequestDTO); // Lot 상태 변경
    }
}
