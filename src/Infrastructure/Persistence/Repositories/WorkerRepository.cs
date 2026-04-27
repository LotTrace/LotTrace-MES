using LotTrace_MES.Domain.Interfaces;
using LotTrace_MES.src.Domain.Entity;
using LotTrace_MES.src.Infrastructure.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace LotTrace_MES.src.Infrastructure.Persistence.Repositories
{
    public class WorkerRepository : GenericRepository<Worker>, IWorkerRepository // 상속을 받았기 떄문에 _context를 다시 정의할 필요가 없다.
    {
        public WorkerRepository(AppDbContext context) : base(context) // 상속받은 부모 파일에 context를 넘겨준다.
        {

        }

        public async Task<IEnumerable<Worker>> GetByDepartmentAsync(string department) // 부서별 작업자 조회
        {
            return await _dbSet.Where(w => w.Department == department).ToListAsync();
        }

        public async Task<Worker?> GetByEmployeeNumberAsync(int employeeNumber) // 사번으로 작업자 조회
        {
            return await _dbSet.FirstOrDefaultAsync(w => w.EmployeeNumber == employeeNumber);
        }

        public async Task<Worker?> GetByNameAsync(string name) // 이름으로 작업자 조회
        {
            return await _dbSet.FirstOrDefaultAsync(w => w.WorkerName == name);
        }
    }
}
