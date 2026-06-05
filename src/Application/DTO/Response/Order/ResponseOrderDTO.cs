namespace LotTrace_MES.src.Application.DTO.Response.Order
{
    public class ResponseOrderDTO
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public required string ProductName { get; set; }
        public required string OrderStatus { get; set; }
        public int PlanQuantity { get; set; }
        public int ProducedQuantity { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
