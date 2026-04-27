namespace LotTrace_MES.src.Application.DTO.Response.Lot
{
    public class CreateRequestLotDTO
    {
        public required string Barcode { get; set; }
        public required int ProductId { get; set; }
        public required int WorkerId { get; set; }
        public required int LineId { get; set; }
    }
}
