namespace LotTrace_MES.src.Application.DTO.Response.Worker
{
    public class ResponseWorkerDTO
    {
        public int WorkerId { get; set; }
        public required string EmployeeNumber { get; set; }
        public required string WorkerName { get; set; }
        public required string Department { get; set; }
        public required string Role { get; set; }
    }
}
