namespace LotTrace_MES.src.Domain.Entity
{
    public class Order
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public required Product Product { get; set; }

        public string OrderStatus { get; set; } = "Pending";
        public int PlanQuantity { get; set; }
        public int ProducedQuantity { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? EndAt { get; set; } 
    }
}