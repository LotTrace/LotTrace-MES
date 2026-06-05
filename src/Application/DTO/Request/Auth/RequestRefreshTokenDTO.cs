namespace LotTrace_MES.src.Application.DTO.Request.Auth
{
    public class RefreshTokenRequest
    {
        public required string ExpiredToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
