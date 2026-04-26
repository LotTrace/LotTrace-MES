using LotTrace_MES.Domain.Interfaces;
using LotTrace_MES.src.Application.Interfaces;
using LotTrace_MES.src.Domain.Entity;

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
        public async Task<Worker?> CreateWorkerAsync(int employeeNumber, string name, string department)
        {
            try
            {
                var existingWorker = await _workerRepository.GetByEmpolyeeNumberAsync(employeeNumber);
                if(existingWorker != null)
                {
                    _logger.LogWarning("Worker with EmployeeNumber: {EmployeeNumber} already exists.", employeeNumber);
                    return null;
                }

                var worker = new Worker
                {
                    EmployeeNumber = employeeNumber,
                    WorkerName = name,
                    Department = department
                };

                await _workerRepository.AddAsync(worker);
                await _workerRepository.SaveChangesAsync();

                return worker;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating worker with EmployeeNumber: {EmployeeNumber}", employeeNumber);
                throw;
            }
        }

        public async Task<IEnumerable<Worker>> GetAllWorkersAsync()
        {
            try
            {
                var workers = await _workerRepository.GetAllAsync();
                return workers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all workers.");
                throw;
            }
        }

        public async Task<Worker?> GetWorkerByEmployeeNumberAsync(int employeeNumber)
        {
            try
            {
                var worker = await _workerRepository.GetByEmpolyeeNumberAsync(employeeNumber);
                if (worker == null)
                {
                    _logger.LogInformation($"Worker with EmployeeNumber: {employeeNumber} not found.");
                    return null;
                }
                return worker;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving worker with EmployeeNumber: {EmployeeNumber}", employeeNumber);
                throw;
            }
        }

        public async Task<Worker?> GetWorkerByNameAsync(string name)
        {
            try
            {
                var worker = await _workerRepository.GetByNameAsync(name);
                if (worker == null)
                {
                    _logger.LogInformation($"Worker with Name: {name} not found.");
                    return null;
                }
                return worker;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving worker with Name: {Name}", name);
                throw;
            }
        }

        public async Task<IEnumerable<Worker>> GetWorkersByDepartmentAsync(string department)
        {
            try
            {
                var workers = await _workerRepository.GetByDepartmentAsync(department);
                return workers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving workers in Department: {Department}", department);
                throw;
            }
        }
    }
}
