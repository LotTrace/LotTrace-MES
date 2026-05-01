using LotTrace_MES.src.Application.DTO.Request.Line;
using LotTrace_MES.src.Application.DTO.Response.Line;
using LotTrace_MES.src.Application.Interfaces;
using LotTrace_MES.src.Domain.Entity;
using LotTrace_MES.src.Domain.Enum;
using Microsoft.AspNetCore.Mvc;

namespace LotTrace_MES.src.Api
{
    [Route("api/line")]
    [ApiController]
    public class LineController : ControllerBase
    {
        private readonly ILineService _lineService;

        public LineController(ILineService lineService)
        {
            _lineService = lineService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResponseLineDTO>>> GetAllLines()
        {
            var lines = await _lineService.GetAllLinesAsync();
            return Ok(lines);
        }

        [HttpGet("{lineId}")]
        public async Task<ActionResult<ResponseLineDTO>> GetLineById(int lineId)
        {
            var line = await _lineService.GetByLineIdAsync(lineId);
            if (line == null) return NotFound($"Line with ID {lineId} not found.");
            return Ok(line);
        }

        [HttpGet("state/{state}")]
        public async Task<ActionResult<IEnumerable<ResponseLineDTO>>> GetLineByState(LineState state)
        {
            var lines = await _lineService.GetByLineStateAsync(state);
            return Ok(lines);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseLineDTO>> CreateLine([FromBody] CreateRequestLineDTO createRequestLineDTO)
        {
            var line = await _lineService.CreateLineAsync(createRequestLineDTO);
            if(line == null) return BadRequest("Failed to create line."); // 400 응답

            return CreatedAtAction(nameof(GetLineById), new { lineId = line.LineId }, line); // 201 응답
        }

        [HttpDelete("{lineId}")]
        public async Task<ActionResult<bool>> DeleteLine(int lineId)
        {
            var success = await _lineService.DeletedByIdAsync(lineId);
            if (success == false) return BadRequest($"Failed to delete line with ID {lineId}.");

            return Ok(new { message = "Deleted successfully" });
        }

        [HttpPut("{lineId}")]
        public async Task<ActionResult<bool>> UpdateLineStateAsync(UpdateRequestLineDTO updateRequestLineDTO)
        {
            var success = await _lineService.UpdateLineStateAsync(updateRequestLineDTO);
            if (success == false) return BadRequest($"Failed to Update line State wit ID {updateRequestLineDTO.LineId}.");

            return Ok(new { message = "Update State successfully" });
        }


    }
}
