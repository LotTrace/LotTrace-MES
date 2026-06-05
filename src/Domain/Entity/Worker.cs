namespace LotTrace_MES.src.Domain.Entity
{
    public class Worker
    {
        // 작업자 정보
        public int WorkerId { get; set; }
        public required string EmployeeNumber { get; set; }
        public required string WorkerName { get; set; }
        public required string Department { get; set; }
        public string Role { get; set; } = "Operator";
        public string PasswordHash { get; set; } = string.Empty;
    }
}
