using LotTrace_MES.src.Application.DTO.Request.Worker;
using LotTrace_MES.src.Application.DTO.Response.Worker;
using LotTrace_MES.src.Application.Interfaces;
using LotTrace_MES.src.Domain.Entity;
using LotTrace_MES.src.Domain.Interfaces;


namespace LotTrace_MES.src.Application.Service
{
    public class WorkerService : IWorkerService
    {
        private readonly IWorkerRepository _workerRepository;
        private readonly ILogger<WorkerService> _logger;

        public WorkerService(IWorkerRepository workerRepository, ILogger<WorkerService> logger)
        {
            _workerRepository = workerRepository;
            _logger = logger;
        }
        public async Task<ResponseWorkerDTO?> CreateWorkerAsync(RequestWorkerDTO createRequestWorkerDTO)
        {
            try
            {
                var existingWorker = await _workerRepository.GetByEmployeeNumberAsync(createRequestWorkerDTO.EmployeeNumber); // 이미 등록되있는 Worker의 경우 null 반환
                if(existingWorker != null)
                {
                    _logger.LogWarning("Worker with EmployeeNumber: {EmployeeNumber} already exists.", createRequestWorkerDTO.EmployeeNumber);
                    return null;
                }

                var worker = new Worker
                {
                    EmployeeNumber = createRequestWorkerDTO.EmployeeNumber,
                    WorkerName = createRequestWorkerDTO.Name,
                    Department = createRequestWorkerDTO.Department,
                    Role = createRequestWorkerDTO.Role,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(createRequestWorkerDTO.Password ?? "1234") // 기본값 1234
                };

                await _workerRepository.AddAsync(worker);
                await _workerRepository.SaveChangesAsync();

                var response = new ResponseWorkerDTO
                {
                    WorkerId = worker.WorkerId,
                    EmployeeNumber = worker.EmployeeNumber,
                    WorkerName = worker.WorkerName,
                    Department = worker.Department,
                    Role = worker.Role
                }; 
                
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating worker with EmployeeNumber: {EmployeeNumber}", createRequestWorkerDTO.EmployeeNumber);
                throw;
            }
        }

        public async Task<bool> DeleteWorker(int workerId)
        {
            try
            {
                var worker = await _workerRepository.GetByIdAsync(workerId);
                if (worker == null)
                {
                    _logger.LogWarning($"Worker with ID {workerId} not found for deletion");
                    return false;
                }

                _workerRepository.Delete(worker);
                await _workerRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting worker with ID {workerId}");
                return false;
            }
        }

        public async Task<bool> UpdateWorker(int workerId, RequestWorkerDTO RequestDTO)
        {
            try
            {
                var worker = await _workerRepository.GetByIdAsync(workerId);
                if (worker == null)
                {
                    _logger.LogWarning($"Worker with ID {workerId} not found for update");
                    return false;
                }
                worker.WorkerName = RequestDTO.Name ?? worker.WorkerName;
                worker.Department = RequestDTO.Department ?? worker.Department;
                worker.Role = RequestDTO.Role ?? worker.Role;

                if (!string.IsNullOrEmpty(RequestDTO.Password))
                {
                    worker.PasswordHash = BCrypt.Net.BCrypt.HashPassword(RequestDTO.Password);
                }

                _workerRepository.Updated(worker);
                await _workerRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating worker with ID {workerId}");
                return false;
            }
        }

        public async Task<IEnumerable<ResponseWorkerDTO>> GetAllWorkersAsync()
        {
            try
            {
                var workers = await _workerRepository.GetAllAsync();
                return workers.Select(worker => new ResponseWorkerDTO
                {
                    WorkerId = worker.WorkerId,
                    EmployeeNumber = worker.EmployeeNumber,
                    WorkerName = worker.WorkerName,
                    Department = worker.Department,
                    Role = worker.Role,
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all workers.");
                return Enumerable.Empty<ResponseWorkerDTO>();
            }
        }

        public async Task<ResponseWorkerDTO?> GetWorkerByEmployeeNumberAsync(string employeeNumber)
        {
            try
            {
                var worker = await _workerRepository.GetByEmployeeNumberAsync(employeeNumber);
                if (worker == null)
                {
                    _logger.LogInformation($"Worker with EmployeeNumber: {employeeNumber} not found.");
                    return null;
                }

                var response = new ResponseWorkerDTO
                {
                    WorkerId = worker.WorkerId,
                    EmployeeNumber = worker.EmployeeNumber,
                    WorkerName = worker.WorkerName,
                    Department = worker.Department,
                    Role = worker.Role,
                };

                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving worker with EmployeeNumber: {EmployeeNumber}", employeeNumber);
                throw;
            }
        }

        public async Task<ResponseWorkerDTO?> GetWorkerByNameAsync(string name)
        {
            try
            {
                var worker = await _workerRepository.GetByNameAsync(name);
                if (worker == null)
                {
                    _logger.LogInformation($"Worker with Name: {name} not found.");
                    return null;
                }

                var response = new ResponseWorkerDTO
                {
                    WorkerId = worker.WorkerId,
                    EmployeeNumber = worker.EmployeeNumber,
                    WorkerName = worker.WorkerName,
                    Department = worker.Department,
                    Role = worker.Role
                };

                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving worker with Name: {Name}", name);
                throw;
            }
        }

        public async Task<ResponseWorkerDTO?> GetWorkerByIdAsync(int workerId)
        {
            try
            {
                var worker = await _workerRepository.GetByIdAsync(workerId);
                if (worker == null)
                {
                    _logger.LogInformation($"Worker with ID: {workerId} not found.");
                    return null;
                }

                var response = new ResponseWorkerDTO
                {
                    WorkerId = worker.WorkerId,
                    EmployeeNumber = worker.EmployeeNumber,
                    WorkerName = worker.WorkerName,
                    Department = worker.Department,
                    Role = worker.Role
                };

                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving worker with ID: {WorkerId}", workerId);
                return null;
            }
        }

        public async Task<IEnumerable<ResponseWorkerDTO>> GetWorkersByDepartmentAsync(string department)
        {
            try
            { 
                var workers = await _workerRepository.GetByDepartmentAsync(department);
                return workers.Select(worker => new ResponseWorkerDTO
                {
                    WorkerId = worker.WorkerId,
                    EmployeeNumber = worker.EmployeeNumber,
                    WorkerName = worker.WorkerName,
                    Department = worker.Department,
                    Role = worker.Role
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving workers in Department: {Department}", department);
                return Enumerable.Empty<ResponseWorkerDTO>();
            }
        }

        public async Task<ResponseWorkerDTO?> VerifyWorkerAsync(string employeeNumber, string password)
        {
            var worker = await _workerRepository.GetByEmployeeNumberAsync(employeeNumber);
            if (worker == null || !BCrypt.Net.BCrypt.Verify(password, worker.PasswordHash))
            {
                return null;
            }

            return new ResponseWorkerDTO
            {
                WorkerId = worker.WorkerId,
                EmployeeNumber = worker.EmployeeNumber,
                WorkerName = worker.WorkerName,
                Department = worker.Department,
                Role = worker.Role
            };
        }
    }
}
