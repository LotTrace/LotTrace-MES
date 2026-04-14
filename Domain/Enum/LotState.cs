namespace LotTrace_MES.Domain.Enum
{
    public enum LotState
    {
        Created, // LotId 생산 (입고 전)
        Wait, // 공정 투입 대기
        Run, // 공정 가동 중
        Hold, // 보류
        Complete // 최종 완료
    }
}
