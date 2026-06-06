using LotTrace_MES.src.Domain.Entity;
using LotTrace_MES.src.Domain.Interfaces.Common;

namespace LotTrace_MES.src.Domain.Interfaces
{
    public interface IWorkerRepository : IRepository<Worker>
    {
        Task<Worker?> GetByNameAsync(string name);
        Task<Worker?> GetByEmployeeNumberAsync(string employeeNumber);
        Task<IEnumerable<Worker>> GetByDepartmentAsync(string department);
    }
}