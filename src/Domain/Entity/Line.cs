using LotTrace_MES.src.Domain.Enum;

namespace LotTrace_MES.src.Domain.Entity
{
    public class Line
    {
        // 라인 정보
        public int LineId { get; set; }
        public string? LineName { get; set; }
        public string? Description { get; set; }
        public LineState CurrentState { get; set; }
    }
}
