using LotTrace_MES.src.Domain.Enum;

namespace LotTrace_MES.src.Application.DTO.Response.Lot
{
    public class CreateResponseLotDTO
    {
        public int LotId { get; set; }
        public required string Barcode { get; set; }
        public required string ProductName { get; set; }
        public LotState CurrentState { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
