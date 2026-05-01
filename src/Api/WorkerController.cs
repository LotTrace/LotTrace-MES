using LotTrace_MES.src.Application.DTO.Request.Worker;
using LotTrace_MES.src.Application.DTO.Response.Worker;
using LotTrace_MES.src.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LotTrace_MES.src.Api
{
    [Route("api/worker")]
    [ApiController]
    public class WorkerController : ControllerBase
    {
        private readonly IWorkerService _workerService;

        public WorkerController(IWorkerService workerService)
        {
            _workerService = workerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResponseWorkerDTO>>> GetAllWorkers()
        {
            var workers = await _workerService.GetAllWorkersAsync();
            return Ok(workers);
        }

        [HttpGet("department/{department}")]
        public async Task<ActionResult<IEnumerable<ResponseWorkerDTO>>> GetAllWorkersByDepartment(string department)
        {
            var workers = await _workerService.GetWorkersByDepartmentAsync(department);
            return Ok(workers);
        }

        [HttpGet("{workerId}")]
        public async Task<ActionResult<ResponseWorkerDTO>> GetWorkerById(int workerId)
        {
            var worker = await _workerService.GetWorkerByIdAsync(workerId);
            if (worker == null)
            {
                return NotFound();
            }
            return Ok(worker);
        }

        [HttpGet("employeeNumber/{employeeNumber}")]
        public async Task<ActionResult<ResponseWorkerDTO>> GetWorkerByEmployeeNumber(int employeeNumber)
        {
            var worker = await _workerService.GetWorkerByEmployeeNumberAsync(employeeNumber);
            if (worker == null)
            {
                return NotFound();
            }
            return Ok(worker);
        }

        [HttpGet("name/{name}")]
        public async Task<ActionResult<ResponseWorkerDTO>> GetWorkerByName(string name)
        {
            var worker = await _workerService.GetWorkerByNameAsync(name);
            if (worker == null)
            {
                return NotFound();
            }
            return Ok(worker);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseWorkerDTO>> CreateWorker([FromBody] RequestWorkerDTO createRequestWorkerDTO)
        {
            var worker = await _workerService.CreateWorkerAsync(createRequestWorkerDTO);
            if (worker == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(GetWorkerById), new { workerId = worker.WorkerId }, worker);

        }

        [HttpDelete("{workerId}")]
        public async Task<ActionResult<bool>> DeleteWorker(int workerId)
        {
            var success = await _workerService.DeleteWorker(workerId);
            if (!success)
            {
                return BadRequest($"Failed to delete worker with ID {workerId}.");
            }
            return Ok(new { message = "Deleted successfully" });
        }

        [HttpPut("{workerId}")]
        public async Task<ActionResult<bool>> UpdateWorker(int workerId, [FromBody] RequestWorkerDTO updateRequestWorkerDTO)
        {
            var success = await _workerService.UpdateWorker(workerId, updateRequestWorkerDTO);
            if (!success)
            {
                return BadRequest($"Failed to update worker with ID {workerId}.");
            }
            return Ok(new { message = "Updated successfully" });
        }
    }
}
