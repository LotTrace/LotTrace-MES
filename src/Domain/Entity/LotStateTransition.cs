using LotTrace_MES.src.Domain.Enum;

namespace LotTrace_MES.src.Domain.Entity
{
    public class LotStateTransition
    {
        public int Id { get; set; }
        public LotState FromState { get; set; }
        public LotState ToState { get; set; }
        public bool IsAllowed { get; set; }
        public string? Description { get; set; }
    }
}
