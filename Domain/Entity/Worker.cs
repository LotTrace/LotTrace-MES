namespace LotTrace_MES.Domain.Entity
{
    public class Worker
    {
        // 작업자 정보
        public int WorkerId { get; set; }
        public string? WorkerName { get; set; }
        public string? Department { get; set; }
    }
}
