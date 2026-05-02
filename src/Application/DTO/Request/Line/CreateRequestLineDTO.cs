using LotTrace_MES.src.Domain.Enum;

namespace LotTrace_MES.src.Application.DTO.Request.Line
{
    public class CreateRequestLineDTO
    {
        public required string LineName { get; set; }
        public required string Description { get; set; }
        public LineState CurrentState { get; set; }
    }
}
