namespace LotTrace_MES.src.Application.DTO.Response.Worker
{
    public class ResponseWorkerDTO
    {
        public int WorkerId { get; set; }
        public int EmployeeNumber { get; set; }
        public required string WorkerName { get; set; }
        public required string Department { get; set; }
    }
}
