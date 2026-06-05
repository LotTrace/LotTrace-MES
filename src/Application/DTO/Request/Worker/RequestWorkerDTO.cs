namespace LotTrace_MES.src.Application.DTO.Request.Worker
{
    public class RequestWorkerDTO
    {
        public required string EmployeeNumber { get; set; }
        public required string Name { get; set; }
        public required string Department { get; set; }
        public required string Role { get; set; }
    }
}
