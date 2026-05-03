using LotTrace_MES.src.Application.DTO.Request.Lot;
using LotTrace_MES.src.Application.DTO.Response.Lot;
using LotTrace_MES.src.Application.Interfaces;
using LotTrace_MES.src.Domain.Enum;
using Microsoft.AspNetCore.Mvc;

namespace LotTrace_MES.src.Api
{
    [Route("api/lot")]
    [ApiController]
    public class LotController : ControllerBase
    {
        private readonly ILotService _lotService;

        public LotController(ILotService lotService)
        {
            _lotService = lotService;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseLotDTO>> CreateLot([FromBody] CreateRequestLotDTO createRequestLotDTO)
        {
            var result = await _lotService.CreateLotAsync(createRequestLotDTO);
            if (result == null) return BadRequest("Failed to create lot with the provided information."); // 400 응답

            return CreatedAtAction(nameof(GetLotsById), new { lotId = result.LotId }, result); // 201 응답 + 조회 경로 제공
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResponseLotDTO>>> GetLots()
        {
            var result = await _lotService.GetLotsAsync();

            return Ok(result);
        }

        [HttpGet("{lotId}")]
        public async Task<ActionResult<ResponseLotDTO>> GetLotsById(int lotId)
        {
            var result = await _lotService.GetLotByIdAsync(lotId);
            if (result == null) return NotFound($"Not found lots for lot ID {lotId}"); // 404 응답

            return Ok(result);
        }

        [HttpGet("barcode/{barcode}")]
        public async Task<ActionResult<ResponseLotDTO>> GetLotsByBarcode(string barcode)
        {
            var result = await _lotService.GetLotByBarcodeAsync(barcode);
            if (result == null) return NotFound($"Not found lots for barcode {barcode}");

            return Ok(result);
        }

        [HttpPost("next")]
        public async Task<ActionResult<bool>> MoveNextStep([FromBody] ChangeRequestLotDTO changeRequestLotDTO)
        {
            Console.WriteLine(changeRequestLotDTO.Barcode, changeRequestLotDTO.WorkerId);
            var success = await _lotService.MoveNextStepAsync(changeRequestLotDTO);
            if (success == false) return BadRequest();

            return Ok(new {message = "Moved to next step"});
        }

        [HttpPost("state")]
        public async Task<ActionResult<ResponseLotDTO>> ChangeState([FromBody] ChangeRequestLotDTO changeRequestLotDTO, [FromQuery] LotState newState)
        {
            var success = await _lotService.ChangeStateAsync(newState, changeRequestLotDTO);
            if (success == false) return BadRequest();

            return Ok(new { message = "Changed state successfully" });
        }

        [HttpDelete("{lotId}")]
        public async Task<ActionResult<bool>> DeleteLot(int lotId)
        {
            var success = await _lotService.DeleteLotAsync(lotId);
            if (success == false) return BadRequest();

            return Ok(new { message = "Deleted successfully" });
        }

        [HttpGet("line/{lineId}")]
        public async Task<ActionResult<IEnumerable<ResponseLotDTO>>> GetLotsByLineId(int lineId)
        {
            var result = await _lotService.GetLotsByLineIdAsync(lineId);
            if (result == null) return NotFound($"Not found lots for line ID {lineId}");

            return Ok(result);
        }

        [HttpGet("state/{state}")]
        public async Task<ActionResult<IEnumerable<ResponseLotDTO>>> GetLotsByState(LotState state)
        {
            var result = await _lotService.GetLotsByStateAsync(state);
            return Ok(result);
        }
    }
}
