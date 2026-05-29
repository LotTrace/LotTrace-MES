using LotTrace_MES.src.Application.DTO.Request.Order;
using LotTrace_MES.src.Application.DTO.Response.Order;
using LotTrace_MES.src.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LotTrace_MES.src.Api
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResponseOrderDTO>>> GetAllOrders()
        {
            var response = await _orderService.GetAllOrdersAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseOrderDTO>> GetOrderById(int id)
        {
            var response = await _orderService.GetOrderDetailsAsync(id);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseOrderDTO>> CreateOrder([FromBody] RequestOrderDTO requestOrderDTO)
        {
            var response = await _orderService.CreateOrderAsync(requestOrderDTO);
            if(response == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(GetOrderById), new { id = response.OrderId }, response);
        }

        [HttpPost("{orderId}/start")]
        public async Task<ActionResult> StartOrder(int orderId)
        {
            var success = await _orderService.StartOrderAsync(orderId);
            if (!success)
            {
                return BadRequest();
            }
            return Ok(new { message = "생산지시 가동이 시작되었습니다." });
        }

        [HttpPost("{orderId}/complete")]
        public async Task<ActionResult> CompleteOrder(int orderId)
        {
            var success = await _orderService.CompleteOrderAsync(orderId);
            if (!success)
            {
                return BadRequest();
            }
            return Ok(new { message = "생산지시 마감이 완료되었습니다." });
        }

        [HttpPut("{orderId}/change-quantity")]
        public async Task<ActionResult> ChangeQty(int orderId, [FromBody] int newQuantity)
        {
            var success = await _orderService.UpdateProductQtyAsync(orderId, newQuantity);
            if (!success)
            {
                return BadRequest(new { message = "수량 업데이트 실패"});
            }
            return Ok(new { message = "생산 실적 수량이 업데이트되었습니다.", currentQty = newQuantity });
        }

    }
}
