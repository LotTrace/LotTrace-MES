using LotTrace_MES.src.Application.DTO.Request.Order;
using LotTrace_MES.src.Application.DTO.Response.Order;

namespace LotTrace_MES.src.Application.Interfaces
{
    public interface IOrderService
    {
        Task<ResponseOrderDTO> CreateOrderAsync(RequestOrderDTO requestOrderDTO); 
        Task<bool> StartOrderAsync(int orderId);
        Task<bool> CompleteOrderAsync(int orderId);
        Task<bool> UpdateProductQtyAsync(int orderId, int newQuantity);
        Task<IEnumerable<ResponseOrderDTO>> GetAllOrdersAsync();
        Task<ResponseOrderDTO> GetOrderDetailsAsync(int orderId);
    }
}
