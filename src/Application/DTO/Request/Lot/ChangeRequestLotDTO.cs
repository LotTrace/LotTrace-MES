using LotTrace_MES.src.Domain.Enum;

namespace LotTrace_MES.src.Application.DTO.Request.Lot
{
    public class ChangeRequestLotDTO
    {
        public required string Barcode { get; set; }
        public required int WorkerId { get; set; }
        public required LotState TargetState {  get; set; }
    }
}
