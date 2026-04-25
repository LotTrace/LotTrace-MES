using LotTrace_MES.Domain.Interfaces;
using LotTrace_MES.src.Domain.Entity;
using LotTrace_MES.src.Infrastructure.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace LotTrace_MES.src.Infrastructure.Persistence.Repositories
{
    public class WorkerRepository : GenericRepository<Worker>, IWorkerRepository
    {
        public WorkerRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Worker>> GetByDepartmentAsync(string department)
        {
            return await _dbSet.Where(w => w.Department == department).ToListAsync();
        }

        public async Task<Worker?> GetByEmpolyeeNumberAsync(int empolyeeNumber)
        {
            return await _dbSet.FirstOrDefaultAsync(w => w.EmployeeNumber == empolyeeNumber);
        }

        public async Task<Worker?> GetByNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(w => w.WorkerName == name);
        }
    }
}
