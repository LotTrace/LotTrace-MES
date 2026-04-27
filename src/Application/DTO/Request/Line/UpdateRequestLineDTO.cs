using LotTrace_MES.src.Domain.Enum;

namespace LotTrace_MES.src.Application.DTO.Request.Line
{
    public class UpdateRequestLineDTO
    {
        public int LineId { get; set; }
        public LineState NewState { get; set; }
        public int WorkerId { get; set; }
    }
}
