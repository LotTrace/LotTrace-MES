using LotTrace_MES.src.Application.DTO.Request.Line;
using LotTrace_MES.src.Application.DTO.Response.Line;
using LotTrace_MES.src.Domain.Enum;

namespace LotTrace_MES.src.Application.Interfaces
{
    public interface ILineService
    {
        Task<IEnumerable<ResponseLineDTO>> GetAllLinesAsync(); // 모든 라인 조회
        Task<ResponseLineDTO> CreateLineAsync(CreateRequestLineDTO createRequestLineDTO); // 라인 생성
        Task<bool> DeletedByIdAsync(int lineId); // 라인 삭제
        Task<bool> UpdateLineStateAsync(UpdateRequestLineDTO updateRequest); // 특정 라인의 상태를 변경
        Task<IEnumerable<ResponseLineDTO>> GetByLineStateAsync(LineState state); // 라인 상태에 따른 조회
        Task<ResponseLineDTO?> GetByLineIdAsync(int lineId); // 라인 아이디로 현재 상태 조회
    }
}
