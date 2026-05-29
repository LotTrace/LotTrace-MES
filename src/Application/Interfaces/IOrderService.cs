using LotTrace_MES.src.Domain.Entity;

namespace LotTrace_MES.src.Application.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(int productId, int quantity); 
        Task<bool> StartOrderAsync(int orderId);
        Task<bool> CompleteOrderAsync(int orderId);
        Task<bool> UpdateProductQtyAsync(int orderId, int newQuantity);
    }
}
