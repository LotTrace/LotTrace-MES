
using LotTrace_MES.src.Application.DTO.Response.Material;
using LotTrace_MES.src.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LotTrace_MES.src.Api
{
    [Authorize]
    [Route("api/material")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly IMaterialService _materialService;

        public MaterialController(IMaterialService materialService)
        {
            _materialService = materialService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResponseMaterialDTO>>> GetAllMaterials()
        {
            var response = await _materialService.GetAllMaterialsAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseMaterialDTO>> GetMaterialById(int id)
        {
            var response = await _materialService.GetMaterialByIdAsync(id);
            if (response == null)
            {
                return NotFound(new { message = $"자재 ID {id}를 찾을 수 없습니다." });
            }
            return Ok(response);
        }

   
        [HttpGet("low-stock")]
        public async Task<ActionResult<IEnumerable<ResponseMaterialDTO>>> GetLowStockMaterials()
        {
            var response = await _materialService.GetDangerousStockMaterialsAsync();
            return Ok(response);
        }

        [HttpPost("{materialId}/inbound")]
        public async Task<ActionResult> InboundMaterial(int materialId, [FromBody] int quantity)
        {
            var success = await _materialService.InboundMaterialAsync(materialId, quantity);
            if (!success)
            {
                return BadRequest(new { message = "자재 입고 처리에 실패했습니다. 자재 ID를 확인하세요." });
            }
            return Ok(new { message = "자재 입고가 정상적으로 반영되었습니다." });
        }

        [HttpPost("{materialId}/deduct")]
        public async Task<ActionResult> DeductStock(int materialId, [FromBody] int quantity) 
        {
            var success = await _materialService.DeductStockAsync(materialId, quantity);
            if (!success)
            {
                return BadRequest(new { message = "자재 차감 처리에 실패했습니다." });
            }
            return Ok(new { message = "생산 투입 자재 차감이 완료되었습니다." });
        }

        [HttpPut("{materialId}/adjust")]
        public async Task<ActionResult> AdjustStock(int materialId, [FromBody] int newStockQty)
        {
            var success = await _materialService.AdjustStockAsync(materialId, newStockQty);
            if (!success)
            {
                return BadRequest(new { message = "관리자 재고 강제 조정에 실패했습니다." });
            }
            return Ok(new { message = "자재 실사 재고 조정이 완료되었습니다." });
        }
    }
}