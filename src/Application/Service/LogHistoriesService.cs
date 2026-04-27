using LotTrace_MES.Domain.Interfaces;
using LotTrace_MES.src.Application.DTO.Response.LogHistories;
using LotTrace_MES.src.Application.Interfaces;
using LotTrace_MES.src.Domain.Entity;
using LotTrace_MES.src.Domain.Interfaces;

namespace LotTrace_MES.src.Application.Service
{
    public class LogHistoriesService : ILogHistoriesService
    {
        private readonly ILotRepository _lotRepository;
        private readonly ILogHistoriesRepository _logHistoriesRepository;
        private readonly ILogger<LogHistoriesService> _logger;

        public LogHistoriesService(ILotRepository lotRepository, ILogHistoriesRepository logHistoriesRepository, ILogger<LogHistoriesService> logger)
        {
            _lotRepository = lotRepository;
            _logHistoriesRepository = logHistoriesRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<ResponseLogDTO>> GetLogHistoriesByBarcodeAsync(string barcode)
        {
            try
            {
                var lot = await _lotRepository.GetByBarcodeAsync(barcode);
                if (lot == null)
                {
                    _logger.LogWarning("Lot with barcode {Barcode} not found.", barcode);
                    throw new KeyNotFoundException($"Lot with barcode {barcode} not found.");
                }

                var logs = await _logHistoriesRepository.GetHistoryByLotIdAsync(lot.LotId);


                return logs.Select(log => new ResponseLogDTO
                {
                    LogHistoriesId = log.LogHistoriesId,
                    LotId = log.LotId,
                    WorkerId = log.WorkerId,
                    WorkerName = log.Worker?.WorkerName,
                    PrevState = log.PrevState,
                    NewState = log.NewState,
                    EventTime = log.EventTime
                }).ToList();
            }
            catch (KeyNotFoundException)
            {
                throw; 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving log histories for barcode {Barcode}.", barcode);
                throw;
            }
        }

        public async Task<IEnumerable<ResponseLogDTO>> GetLogHistoriesByDateAsync(DateTime start, DateTime end)
        {
            try
            {
                var logs = await _logHistoriesRepository.GetByDateAsync(start, end);


                return logs.Select(log => new ResponseLogDTO
                {
                    LogHistoriesId = log.LogHistoriesId,
                    LotId = log.LotId,
                    WorkerId = log.WorkerId,
                    WorkerName = log.Worker?.WorkerName,
                    PrevState = log.PrevState,
                    NewState = log.NewState,
                    EventTime = log.EventTime
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving log histories between {Start} and {End}.", start, end);
                throw;
            }
        }

        public async Task<IEnumerable<ResponseLogDTO>> GetLogHistoriesByLotIdAsync(int lotId)
        {
            try
            {
                var logs = await _logHistoriesRepository.GetHistoryByLotIdAsync(lotId);

                return logs.Select(log => new ResponseLogDTO
                {
                    LogHistoriesId = log.LogHistoriesId,
                    LotId = log.LotId,
                    WorkerId = log.WorkerId,
                    WorkerName = log.Worker?.WorkerName,
                    PrevState = log.PrevState,
                    NewState = log.NewState,
                    EventTime = log.EventTime
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving log histories for lot ID {LotId}.", lotId);
                throw;
            }
        }
    }
}
