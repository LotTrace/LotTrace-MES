using LotTrace_MES.src.Domain.Entity;

namespace LotTrace_MES.src.Application.Interfaces
{
    public interface IWorkerService
    {
        Task<IEnumerable<Worker>> GetAllWorkersAsync(); // 모든 작업자 조회
        Task<IEnumerable<Worker>> GetWorkersByDepartmentAsync(string department); // 부서별 작업자 조회
        Task<Worker?> GetWorkerByNameAsync(string name); // 이름으로 작업자 조회
        Task<Worker?> GetWorkerByEmployeeNumberAsync(int employeeNumber); // 사번으로 작업자 조회
        Task<Worker> CreateWorkerAsync(int employeeNumber, string name, string department); // 작업자 생성
    }
}
