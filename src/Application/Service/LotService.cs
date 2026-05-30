using LotTrace_MES.src.Application.DTO.Request.Lot;
using LotTrace_MES.src.Application.DTO.Response.Lot;
using LotTrace_MES.src.Application.Interfaces;
using LotTrace_MES.src.Domain.Entity;
using LotTrace_MES.src.Domain.Enum;
using LotTrace_MES.src.Domain.Interfaces;

namespace LotTrace_MES.src.Application.Service
{
    public class LotService : ILotService
    {
        private readonly ILotRepository _lotRepository;
        private readonly ILogHistoriesRepository _logHistoriesRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMaterialRepository _materialRepository;
        private readonly ILogger<LotService> _logger;

        public LotService(ILotRepository lotRepository, ILogHistoriesRepository logHistoriesRepository, IProductRepository productRepository, IOrderRepository OrderRepository, IMaterialRepository materialRepository, ILogger<LotService> logger)
        {
            _lotRepository = lotRepository;
            _logHistoriesRepository = logHistoriesRepository;
            _productRepository = productRepository;
            _orderRepository = OrderRepository;
            _materialRepository = materialRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<ResponseLotDTO>> GetLotsAsync()
        {
            try
            {
                var lots = await _lotRepository.GetAllAsync();
                var response = lots.Select(lot => new ResponseLotDTO
                {
                    LotId = lot.LotId,
                    Barcode = lot.Barcode,
                    ProductName = lot.Product?.ProductName ?? "Unknown",
                    OrderId = lot.OrderId,                              
                    MaterialName = lot.Material?.Name ?? "Unknown",
                    CurrentState = lot.CurrentState,
                    CreatedAt = lot.CreatedAt
                }).ToList();

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all lots.");
                return Enumerable.Empty<ResponseLotDTO>();
            }
        }

        public async Task<ResponseLotDTO?> GetLotByIdAsync(int lotId)
        {
            try
            {
                var lot = await _lotRepository.GetByIdAsync(lotId);
                if (lot == null)
                {
                    _logger.LogWarning("Lot with ID {lotId} not found.", lotId);
                    return null;
                }

                var product = lot.Product;

                if(product == null)
                {
                    product = await _productRepository.GetByIdAsync(lot.ProductId);
                }

                var material = lot.Material;
                if (material == null)
                {
                    material = await _materialRepository.GetByIdAsync(lot.MaterialId);
                }

                var response = new ResponseLotDTO
                {
                    LotId = lot.LotId,
                    Barcode = lot.Barcode,
                    ProductName = product?.ProductName ?? "Unknown",
                    OrderId = lot.OrderId,                            
                    MaterialName = lot.Material?.Name ?? "Unknown",
                    CurrentState = lot.CurrentState,
                    CreatedAt = lot.CreatedAt
                };
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching lot with ID {lotId}.", lotId);
                return null;
            }
        }

        public async Task<ResponseLotDTO?> GetLotByBarcodeAsync(string barcode)
        {
            try
            {
                var lot = await _lotRepository.GetByBarcodeAsync(barcode);
                if (lot == null)
                {
                    _logger.LogWarning("Lot with Barcode {barcode} not found.", barcode);
                    return null;
                }
                var response = new ResponseLotDTO
                {
                    LotId = lot.LotId,
                    Barcode = lot.Barcode,
                    ProductName = lot.Product?.ProductName ?? "Unknown",
                    OrderId = lot.OrderId,                          
                    MaterialName = lot.Material?.Name ?? "Unknown",
                    CurrentState = lot.CurrentState,
                    CreatedAt = lot.CreatedAt
                };
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching lot with Barcode {barcode}.", barcode);
                return null;
            }
        }

        public async Task<bool> ChangeStateAsync(LotState nextState, ChangeRequestLotDTO changeDTO)
        {
            try
            {
                var lot = await _lotRepository.GetByBarcodeAsync(changeDTO.Barcode);
                if (lot == null)
                {
                    _logger.LogWarning("Lot with barcode {Barcode} not found.", changeDTO.Barcode);
                    return false;
                }
                LotState prevState = lot.CurrentState;
                lot.CurrentState = nextState;

                if (await _lotRepository.SaveChangesAsync())
                {
                    var log = new LogHistories
                    {
                        LotId = lot.LotId,
                        PrevState = prevState.ToString(),
                        NextState = nextState.ToString(),
                        WorkerId = changeDTO.WorkerId,
                        EventTime = DateTime.Now
                    };
                    await _logHistoriesRepository.AddAsync(log);
                    await _logHistoriesRepository.SaveChangesAsync();

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing state for lot with barcode {Barcode}.", changeDTO.Barcode);
                return false;
            }
        }

        public async Task<ResponseLotDTO?> CreateLotAsync(CreateRequestLotDTO createDTO)
        {
            try
            {
                var existingLot = await _lotRepository.GetByBarcodeAsync(createDTO.Barcode);
                if (existingLot != null)
                {
                    _logger.LogWarning("Lot with barcode {Barcode} already exists.", createDTO.Barcode);
                    return null;
                }

                var product = await _productRepository.GetByIdAsync(createDTO.ProductId);
                if (product == null)
                {
                    _logger.LogError($"Product not found: {createDTO.ProductId}");
                    return null;
                }

                var material = await _materialRepository.GetByIdAsync(createDTO.MaterialId);
                if (material == null)
                {
                    _logger.LogError($"Material not found: {createDTO.MaterialId}");
                    return null;
                }
                if (material.Stock < createDTO.StartQty)
                {
                    _logger.LogWarning($"Material shortage. Available: {material.Stock}, Requested: {createDTO.StartQty}");
                    throw new InvalidOperationException("창고에 원자재 재고가 부족하여 Lot을 발행할 수 없습니다.");
                }
                material.Stock -= createDTO.StartQty;

                var order = await _orderRepository.GetByIdAsync(createDTO.OrderId);
                if (order == null)
                {
                    _logger.LogError($"Production Order not found: {createDTO.OrderId}");
                    return null;
                }
                order.ProducedQuantity += createDTO.StartQty;

                var newLot = new Lot
                {
                    Barcode = createDTO.Barcode,
                    ProductId = createDTO.ProductId,
                    WorkerId = createDTO.WorkerId,
                    OrderId = createDTO.OrderId,
                    MaterialId = createDTO.MaterialId,
                    StartQty = createDTO.StartQty,
                    Quantity = createDTO.StartQty,
                    LineId = createDTO.LineId,
                    CurrentState = LotState.Created,
                    CreatedAt = DateTime.Now
                };

                await _lotRepository.AddAsync(newLot);
                await _lotRepository.SaveChangesAsync();
                await _materialRepository.SaveChangesAsync();
                await _orderRepository.SaveChangesAsync();

                _logger.LogInformation($"[MES Chain Success] Lot {createDTO.Barcode} created. Material {material.Name} deducted. Order {createDTO.OrderId} progress updated.");
                return await GetLotByBarcodeAsync(newLot.Barcode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating lot with barcode {Barcode}.", createDTO.Barcode);
                throw;
            }
        }


        public async Task<bool> TransitionStateAsync(ChangeRequestLotDTO changeDTO)
        {
            try
            {
                var lot = await _lotRepository.GetByBarcodeForUpdateAsync(changeDTO.Barcode);
                if (lot == null)
                {
                    _logger.LogWarning("Lot with barcode {Barcode} not found.", changeDTO.Barcode);
                    return false;
                }

                LotState prevState = lot.CurrentState;
                LotState nextState = changeDTO.TargetState;

                bool isValidTransition = await _lotRepository.CheckStateTransitionAsync(prevState, nextState);

                if (!isValidTransition)
                {
                    _logger.LogWarning("Invalid state transition from {PrevState} to {NextState} for lot with barcode {Barcode}.", prevState, nextState, changeDTO.Barcode);
                    return false;
                }

                lot.CurrentState = nextState;
                lot.WorkerId = changeDTO.WorkerId;
                lot.LastUpdatedAt = DateTime.Now;

                if (await _lotRepository.SaveChangesAsync())
                {
                    var log = new LogHistories
                    {
                        LotId = lot.LotId,
                        WorkerId = changeDTO.WorkerId,
                        PrevState = prevState.ToString(),
                        NextState = nextState.ToString(),
                        EventTime = DateTime.Now,
                    };
                    await _logHistoriesRepository.AddAsync(log);
                    await _logHistoriesRepository.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in MoveNextStep for {changeDTO.Barcode}");
                return false;
            }
        }

        public async Task<bool> DeleteLotAsync(int lotId)
        {
            try
            {
                var lot = await _lotRepository.GetByIdAsync(lotId);
                if (lot == null)
                {
                    _logger.LogWarning("Lot with ID {lotId} not found.", lotId);
                    return false;
                }
                _lotRepository.Delete(lot);
                return await _lotRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting lot with ID {lotId}.", lotId);
                return false;
            }
        }
        public async Task<IEnumerable<ResponseLotDTO>> GetLotsByLineIdAsync(int lineId)
        {
            try
            {
                var lots = await _lotRepository.GetByLineIdAsync(lineId);
                var response = lots.Select(lot => new ResponseLotDTO
                {
                    LotId = lot.LotId,
                    Barcode = lot.Barcode,
                    ProductName = lot.Product?.ProductName ?? "Unknown",
                    OrderId = lot.OrderId,                           
                    MaterialName = lot.Material?.Name ?? "Unknown",
                    CurrentState = lot.CurrentState,
                    CreatedAt = lot.CreatedAt
                }).ToList();

                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching lots for line ID {lineId}.", lineId);
                return Enumerable.Empty<ResponseLotDTO>();
            }
        }

        public async Task<IEnumerable<ResponseLotDTO>> GetLotsByStateAsync(LotState state)
        {
            try
            {
                var lots = await _lotRepository.GetByStateAsync(state);
                var response = lots.Select(lot => new ResponseLotDTO
                {
                    LotId = lot.LotId,
                    Barcode = lot.Barcode,
                    ProductName = lot.Product?.ProductName ?? "Unknown",
                    OrderId = lot.OrderId,                             
                    MaterialName = lot.Material?.Name ?? "Unknown",
                    CurrentState = lot.CurrentState,
                    CreatedAt = lot.CreatedAt
                }).ToList();
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching lots with state {state}.", state);
                return Enumerable.Empty<ResponseLotDTO>();
            }
        }
    }
}
