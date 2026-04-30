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
        private readonly ILogger<LotService> _logger;

        public LotService(ILotRepository lotRepository, ILogHistoriesRepository logHistoriesRepository, IProductRepository productRepository, ILogger<LotService> logger)
        {
            _lotRepository = lotRepository;
            _logHistoriesRepository = logHistoriesRepository;
            _productRepository = productRepository;
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
                    CurrentState = lot.CurrentState,
                    CreatedAt = lot.CreatedAt ?? DateTime.Now
                });

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
                var response = new ResponseLotDTO
                {
                    LotId = lot.LotId,
                    Barcode = lot.Barcode,
                    ProductName = lot.Product?.ProductName ?? "Unknown",
                    CurrentState = lot.CurrentState,
                    CreatedAt = lot.CreatedAt ?? DateTime.Now
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
                    CurrentState = lot.CurrentState,
                    CreatedAt = lot.CreatedAt ?? DateTime.Now
                };
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching lot with Barcode {barcode}.", barcode);
                return null;
            }
        }

        public async Task<bool> ChangeStateAsync(LotState newState, ChangeRequestLotDTO changeDTO)
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
                lot.CurrentState = newState;

                if (await _lotRepository.SaveChangesAsync())
                {
                    var log = new LogHistories
                    {
                        LotId = lot.LotId,
                        PrevState = prevState.ToString(),
                        NewState = newState.ToString(),
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

                var newLot = new Lot
                {
                    Barcode = createDTO.Barcode,
                    ProductId = createDTO.ProductId,
                    WorkerId = createDTO.WorkerId,
                    LineId = createDTO.LineId,
                    CurrentState = LotState.Created,
                    CreatedAt = DateTime.Now
                };

                await _lotRepository.AddAsync(newLot);
                await _lotRepository.SaveChangesAsync();



                var response = new ResponseLotDTO
                {
                    LotId = newLot.LotId,
                    Barcode = newLot.Barcode,
                    ProductName = product?.ProductName ?? string.Empty,
                    CurrentState = newLot.CurrentState,
                    CreatedAt = newLot.CreatedAt ?? DateTime.Now
                };

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating lot with barcode {Barcode}.", createDTO.Barcode);
                return null;
            }
        }

        public async Task<bool> MoveNextStepAsync(ChangeRequestLotDTO changeDTO)
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
                LotState newState;

                switch (prevState)
                {
                    case LotState.Created: newState = LotState.Wait; break;
                    case LotState.Wait: newState = LotState.Run; break;
                    case LotState.Run: newState = LotState.Complete; break;
                    case LotState.Hold: newState = LotState.Wait; break;
                    default:
                        _logger.LogWarning($"Lot {changeDTO.Barcode} cannot move from {prevState}.");
                        return false;
                }

                lot.CurrentState = newState;

                if (await _lotRepository.SaveChangesAsync())
                {
                    var log = new LogHistories
                    {
                        LotId = lot.LotId,
                        WorkerId = changeDTO.WorkerId,
                        PrevState = prevState.ToString(),
                        NewState = newState.ToString(),
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
                    CurrentState = lot.CurrentState,
                    CreatedAt = lot.CreatedAt ?? DateTime.Now
                });

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
                    CurrentState = lot.CurrentState,
                    CreatedAt = lot.CreatedAt ?? DateTime.Now
                });
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
