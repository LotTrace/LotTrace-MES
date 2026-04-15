using LotTrace_MES.Domain.Enum;

namespace LotTrace_MES.Domain.Entity
{
    public class LogHistories
    {
        // 로그 기록 정보
        public int LogHistoriesId { get; set; }
        public int LotId { get; set; }
        public Lot? Lot { get; set; }
        public int WorkerId { get; set; }
        public Worker? Worker { get; set; }

        // 상태 변경 
        public LotState? PrevState { get; set; } = null;
        public LotState? NewState { get; set; }

        // 수량 변경
        public int? OldQty { get; set; }
        public int? NewQty { get; set; }

        // 로그 기록 시간
        public DateTime? EventTime { get; set; }
    }
}
