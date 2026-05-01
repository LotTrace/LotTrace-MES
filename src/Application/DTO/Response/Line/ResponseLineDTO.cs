using LotTrace_MES.src.Domain.Enum;

namespace LotTrace_MES.src.Application.DTO.Response.Line
{
    public class ResponseLineDTO
    {
        public int LineId { get; set; }
        public required string LineName { get; set; }
        public string? Description { get; set; }
        public LineState CurrentState { get; set; }
    }
}
