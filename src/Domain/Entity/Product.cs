namespace LotTrace_MES.src.Domain.Entity
{
    public class Product
    {
        // 제품 정보
        public int ProductId { get; set; }
        public required string ProductCode { get; set; }
        public required string ProductName { get; set; }
    }
}
