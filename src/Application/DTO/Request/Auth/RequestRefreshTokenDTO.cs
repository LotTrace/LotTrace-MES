namespace LotTrace_MES.src.Application.DTO.Request.Auth
{
    public class RequestRefreshTokenDTO
    {
        public required string ExpiredToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
