using LotTrace_MES.src.Domain.Entity;
using LotTrace_MES.src.Domain.Enum;
using LotTrace_MES.src.Domain.Interfaces.Common;

namespace LotTrace_MES.src.Domain.Interfaces.Services
{
    public interface ILineService : IService<Line>
    {
        Task<IEnumerable<Line>> GetByLineStateAsync(LineState state);
    }
}
