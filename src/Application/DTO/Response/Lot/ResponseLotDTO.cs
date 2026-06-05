using LotTrace_MES.src.Domain.Enum;

namespace LotTrace_MES.src.Application.DTO.Response.Lot
{
    public class ResponseLotDTO
    {
        public int LotId { get; set; }
        public required string Barcode { get; set; }
        public required string ProductName { get; set; }
        public int OrderId { get; set; }
        public string? MaterialName { get; set; } 
        public int Quantity { get; set; }          
        public int StartQty { get; set; }         
        public string? CurrentLocation { get; set; }
        public LotState CurrentState { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
