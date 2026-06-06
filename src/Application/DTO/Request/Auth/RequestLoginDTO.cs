namespace LotTrace_MES.src.Application.DTO.Request.Auth
{
    public class LoginRequestDTO
    {
        public required string EmployeeNumber { get; set; }
        public required string Password { get; set; }
    }
}
