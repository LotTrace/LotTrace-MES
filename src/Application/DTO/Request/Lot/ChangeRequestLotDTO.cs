namespace LotTrace_MES.src.Application.DTO.Request.Lot
{
    public class ChangeRequestDTO
    {
        public required string Barcode { get; set; }
        public required int WorkerId { get; set; }
    }
}
