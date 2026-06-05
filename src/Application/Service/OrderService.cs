using LotTrace_MES.src.Application.DTO.Request.Order;
using LotTrace_MES.src.Application.DTO.Response.Order;
using LotTrace_MES.src.Application.Interfaces;
using LotTrace_MES.src.Domain.Entity;
using LotTrace_MES.src.Domain.Interfaces;

namespace LotTrace_MES.src.Application.Service
{
    public class OrderService : IOrderService
    {

        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<OrderService> _logger;
        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository, ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<bool> CompleteOrderAsync(int orderId)
        {
            try
            {
                var order = await _orderRepository.GetByIdAsync(orderId);
                if (order == null)
                {
                    _logger.LogWarning($"Order with ID {orderId} not found.");
                    return false;
                }

                if (order.OrderStatus != "InProgress")
                {
                    _logger.LogWarning($"Order with ID {orderId} cannot be completed because it is in '{order.OrderStatus}' status.");
                    return false;
                }

                order.OrderStatus = "Completed";
                await _orderRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while completing order ID {orderId}");
                return false;
            }
        }

        public async Task<ResponseOrderDTO> CreateOrderAsync(RequestOrderDTO requestOrderDTO)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(requestOrderDTO.ProductId);

                if(product == null)
                {
                    throw new KeyNotFoundException($"Product with ID {requestOrderDTO.ProductId} was not found.");
                }

                var order = new Order
                {
                    ProductId = requestOrderDTO.ProductId,
                    Product = product,
                    PlanQuantity = requestOrderDTO.PlanQuantity,
                    ProducedQuantity = requestOrderDTO.ProducedQuantity,
                    OrderStatus = "Created",
                };

                await _orderRepository.AddAsync(order);
                await _orderRepository.SaveChangesAsync();

                var response = new ResponseOrderDTO
                {
                    OrderId = order.OrderId,
                    ProductId = order.ProductId,
                    ProductName = order.Product.ProductName,
                    OrderStatus = order.OrderStatus,
                    PlanQuantity = order.PlanQuantity,
                    ProducedQuantity = order.ProducedQuantity,
                    CreatedAt = order.CreatedAt
                };
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while creating order for product ID {requestOrderDTO.ProductId}");
                throw;
            }
        }

        public async Task<IEnumerable<ResponseOrderDTO>> GetAllOrdersAsync()
        {
            try {
                var orders = await _orderRepository.GetOrdersWithProductAsync();

                return orders.Select(order => new ResponseOrderDTO
                {
                    OrderId = order.OrderId,
                    ProductId = order.ProductId,
                    ProductName = order.Product.ProductName,
                    OrderStatus = order.OrderStatus,
                    PlanQuantity = order.PlanQuantity,
                    ProducedQuantity = order.ProducedQuantity,
                    CreatedAt = order.CreatedAt
                }).ToList();

            } catch (Exception ex) {
                _logger.LogError(ex, "Error occurred while retrieving all orders");
                return Enumerable.Empty<ResponseOrderDTO>();
            }
        }

        public async Task<ResponseOrderDTO> GetOrderDetailsAsync(int orderId)
        {
            try
            {
                var order = await _orderRepository.GetByIdAsync(orderId);

                if (order == null)
                {
                    throw new KeyNotFoundException($"Order with ID {orderId} was not found.");
                }

                return new ResponseOrderDTO
                {
                    OrderId = order.OrderId,
                    ProductId = order.ProductId,
                    ProductName = order.Product.ProductName,
                    OrderStatus = order.OrderStatus,
                    PlanQuantity = order.PlanQuantity,
                    ProducedQuantity = order.ProducedQuantity,
                    CreatedAt = order.CreatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving details for order ID {orderId}");
                throw;
            }
        }

        public async Task<bool> StartOrderAsync(int orderId)
        {
            try
            {
                var order = await _orderRepository.GetByIdAsync(orderId);
                if (order == null)
                {
                    _logger.LogWarning($"Order with ID {orderId} not found.");
                    return false;
                }

                if(order.OrderStatus != "Pending" || order.OrderStatus != "Created")
                {
                    _logger.LogWarning($"Order with ID {orderId} cannot be started because it is in '{order.OrderStatus}' status.");
                    return false;
                }

                order.OrderStatus = "InProgress";
                await _orderRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while starting order ID {orderId}");
                return false;
            }
        }

        public async Task<bool> UpdateProductQtyAsync(int orderId, int newQuantity)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                _logger.LogWarning($"Order with ID {orderId} not found.");
                return false;
            }

            order.ProducedQuantity = newQuantity;
            await _orderRepository.SaveChangesAsync();
            return true;
        }
    }
}
