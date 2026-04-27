namespace LotTrace_MES.src.Domain.Enum
{
    public enum LotState
    {
        Created, // LotId 생산
        Wait, // 공정 투입 대기
        Run, // 공정 가동 중
        Hold, // 보류
        Complete // 최종 완료
    }
}
