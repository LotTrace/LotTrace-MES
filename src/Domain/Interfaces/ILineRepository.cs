using LotTrace_MES.src.Domain.Entity;
using LotTrace_MES.src.Domain.Enum;
using LotTrace_MES.src.Domain.Interfaces.Common;

namespace LotTrace_MES.src.Domain.Interfaces
{
    public interface ILineRepository : IRepository<Line>
    {
        Task<IEnumerable<Line>> GetByLineState(LineState state);
    }
}
