using LotTrace_MES.src.Application.DTO.Response.LogHistories;
using LotTrace_MES.src.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LotTrace_MES.src.Api
{
    [Authorize]
    [Route("api/log")]
    [ApiController]
    public class LogHistoriesController : ControllerBase
    {
        private readonly ILogHistoriesService _logService;

        public LogHistoriesController(ILogHistoriesService logService)
        {
            _logService = logService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResponseLogDTO>>> GetAllLogs()
        {
            var logs = await _logService.GetAllLogHistoriesAsync();
            return Ok(logs);
        }

        [HttpGet("lot/{lotId:int}")]
        public async Task<ActionResult<IEnumerable<ResponseLogDTO>>> GetLogsByLotId(int lotId)
        {
            var logs = await _logService.GetLogHistoriesByLotIdAsync(lotId);
            if (!logs.Any()) return NotFound($"No logs found for Lot ID {lotId}.");

            return Ok(logs);
        }

        [HttpGet("barcode/{barcode}")]
        public async Task<ActionResult<IEnumerable<ResponseLogDTO>>> GetLogsByBarcode(string barcode)
        {
            var logs = await _logService.GetLogHistoriesByBarcodeAsync(barcode);
            if (!logs.Any()) return NotFound($"No logs found for Barcode: {barcode}.");

            return Ok(logs);
        }

        [HttpGet("date")]
        public async Task<ActionResult<IEnumerable<ResponseLogDTO>>> GetLogsByDate([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            if (start > end) return BadRequest("Start date cannot be later than end date.");

            var logs = await _logService.GetLogHistoriesByDateAsync(start, end);
            return Ok(logs);
        }
    }
}