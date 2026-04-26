using LotTrace_MES.src.Domain.Entity;
using LotTrace_MES.src.Domain.Interfaces.Common;

namespace LotTrace_MES.src.Domain.Interfaces.Services
{
    public interface IWorkerService : IService<Worker>
    {
        Task<Worker?> GetByNameAsync(string name);
        Task<Worker?> GetByEmployeeNumberAsync(int employeeNumber);
        Task<IEnumerable<Worker>> GetByDepartmentAsync(string department);
    }
}
