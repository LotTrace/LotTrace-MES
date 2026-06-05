using LotTrace_MES.src.Application.DTO.Request.Worker;
using LotTrace_MES.src.Application.DTO.Response.Worker;

namespace LotTrace_MES.src.Application.Interfaces
{
    public interface IWorkerService
    {
        Task<IEnumerable<ResponseWorkerDTO>> GetAllWorkersAsync(); // 모든 작업자 조회
        Task<bool> DeleteWorker(int workerId); // 작업자 삭제
        Task<bool> UpdateWorker(int workerId, RequestWorkerDTO RequestDTO); // 작업자 정보 업데이트
        Task<IEnumerable<ResponseWorkerDTO>> GetWorkersByDepartmentAsync(string department); // 부서별 작업자 조회
        Task<ResponseWorkerDTO?> GetWorkerByNameAsync(string name); // 이름으로 작업자 조회
        Task<ResponseWorkerDTO?> GetWorkerByIdAsync(int workerId); // ID로 작업자 조회
        Task<ResponseWorkerDTO?> GetWorkerByEmployeeNumberAsync(string employeeNumber); // 사번으로 작업자 조회
        Task<ResponseWorkerDTO?> CreateWorkerAsync(RequestWorkerDTO createRequestWorkerDTO); // 작업자 생성
    }
}
