using LotTrace_MES.src.Domain.Entity;

namespace LotTrace_MES.src.Application.DTO.Response.LogHistories
{
    public class ResponseLogDTO
    {
        public int LogHistoriesId { get; set; }
        public int LotId { get; set; }
        public int WorkerId { get; set; }
        public string? WorkerName { get; set; }
        public string? PrevState { get; set; } = null;
        public string? NewState { get; set; }

        public int? OldQty { get; set; }
        public int? NewQty { get; set; }

        public DateTime? EventTime { get; set; }
    }
}
