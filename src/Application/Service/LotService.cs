using LotTrace_MES.Domain.Interfaces;
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
        private readonly ILogger<LotService> _logger;

        public LotService(ILotRepository lotRepository, ILogHistoriesRepository logHistoriesRepository, ILogger<LotService> logger)
        {
            _lotRepository = lotRepository;
            _logHistoriesRepository = logHistoriesRepository;
            _logger = logger;
        }

        public async Task<bool> ChangeStateAsync(string barcode, LotState newState, int workerId)
        {
            try
            {
                var lot = await _lotRepository.GetByBarcodeAsync(barcode);
                if (lot == null)
                {
                    _logger.LogWarning("Lot with barcode {Barcode} not found.", barcode);
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
                        WorkerId = workerId,
                        EventTime = DateTime.Now
                    };
                    await _logHistoriesRepository.AddAsync(log);
                    await _lotRepository.SaveChangesAsync();

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing state for lot with barcode {Barcode}.", barcode);
                return false;
            }
        }

        public async Task<Lot?> CreateLotAsync(string barcode, int productId, int workerId, int lineId)
        {
            try
            {
                var existingLot = await _lotRepository.GetByBarcodeAsync(barcode);
                if (existingLot != null)
                {
                    _logger.LogWarning("Lot with barcode {Barcode} already exists.", barcode);
                    return null;
                }

                var newLot = new Lot
                {
                    Barcode = barcode,
                    ProductId = productId,
                    WorkerId = workerId,
                    LineId = lineId,
                    CurrentState = LotState.Created,
                    CreatedAt = DateTime.Now
                };

                await _lotRepository.AddAsync(newLot);
                await _lotRepository.SaveChangesAsync();

                return newLot;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating lot with barcode {Barcode}.", barcode);
                return null;
            }
        }

        public async Task<bool> MoveNextStepAsync(string barcode, int workerId)
        {
            try 
            {
                var lot = await _lotRepository.GetByBarcodeAsync(barcode);
                if (lot == null)
                {
                    _logger.LogWarning("Lot with barcode {Barcode} not found.", barcode);
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
                        _logger.LogWarning($"Lot {barcode} cannot move from {prevState}.");
                        return false;
                }

                lot.CurrentState = newState;

                if (await _lotRepository.SaveChangesAsync())
                {
                    var log = new LogHistories
                    {
                        LotId = lot.LotId,
                        WorkerId = workerId,
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
                _logger.LogError(ex, $"Error in MoveNextStep for {barcode}");
                return false;
            }
        }
    }
}
