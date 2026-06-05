namespace LotTrace_MES.src.Domain.Entity
{
    public class Auth
    {
        public bool Success { get; set; }
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string WorkerName { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
