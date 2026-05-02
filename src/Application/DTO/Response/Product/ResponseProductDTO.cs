namespace LotTrace_MES.src.Application.DTO.Response.Product
{
    public class ResponseProductDTO
    {
        public int ProductId { get; set; }
        public required string ProductCode { get; set; }
        public required string ProductName { get; set; }
    }
}
