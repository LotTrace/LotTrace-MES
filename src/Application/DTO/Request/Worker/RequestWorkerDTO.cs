namespace LotTrace_MES.src.Application.DTO.Request.Worker
{
    public class RequestWorkerDTO
    {
        public int EmployeeNumber { get; set; }
        public required string Name { get; set; }
        public required string Department { get; set; }
    }
}
