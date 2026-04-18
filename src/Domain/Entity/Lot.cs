using LotTrace_MES.src.Domain.Enum;

namespace LotTrace_MES.src.Domain.Entity
{
    public class Lot
    {
        // 기본 정보
        public int LotId { get; set; }
        public string? Barcode { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int WorkerId { get; set; }
        public Worker? Worker { get; set; }

        // 상태 및 위치 
        public LotState CurrentState { get; set; }
        public string? CurrentLocation { get; set; }
        public int Quantity { get; set; }
        public int StartQty { get; set; }
        public int LineId { get; set; }
        public Line? Line { get; set; }


        // 시간 추적
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }

        // 계보 관리
        public int? ParentLotId { get; set; }
        public Lot? ParentLot { get; set; } // 셀프 참조
    }

}
