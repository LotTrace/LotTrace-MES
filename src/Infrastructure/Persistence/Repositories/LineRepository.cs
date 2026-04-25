using LotTrace_MES.src.Domain.Entity;
using LotTrace_MES.src.Domain.Enum;
using LotTrace_MES.src.Domain.Interfaces;
using LotTrace_MES.src.Infrastructure.Persistence.Repositories.Common;

namespace LotTrace_MES.src.Infrastructure.Persistence.Repositories
{
    public class LineRepository : GenericRepository<Line>, ILineRepository
    {
        public LineRepository(AppDbContext context) : base(context)
        {

        }
        public async Task<IEnumerable<Line>> GetByLineState(LineState state)
        {
            throw new NotImplementedException();
        }
    }
}
