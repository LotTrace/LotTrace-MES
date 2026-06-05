using System.ComponentModel.DataAnnotations;

namespace LotTrace_MES.src.Domain.Entity
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }
        public required string Token { get; set; }
        public required string EmployeeNumber { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsRevoked { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
