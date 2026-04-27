using LotTrace_MES.src.Application.Interfaces;
using LotTrace_MES.src.Domain.Entity;
using LotTrace_MES.src.Domain.Enum;
using LotTrace_MES.src.Domain.Interfaces;

namespace LotTrace_MES.src.Application.Service
{
    public class LineService : ILineService
    {
        private readonly ILineRepository _lineRepository; // DI를 위해 구현체말고 인터페이스를 정의
        private readonly ILogger<LineService> _logger; // 어디서 문제가 발생했는지 로그 타입에 LineService를 붙여서 정의
        public LineService(ILineRepository lineRepository, ILogHistoriesRepository logHistoriesRepository, ILogger<LineService> logger)
        {
            _lineRepository = lineRepository;
            _logHistoriesRepository = logHistoriesRepository;
            _logger = logger;
        }

        public async Task<Line?> GetByLineIdAsync(int lineId)
        {
            try
            {
                var line = await _lineRepository.GetByIdAsync(lineId);
                if (line == null)
                {
                    _logger.LogWarning($"Line not found: ID {lineId}");
                    return null;
                }
                 return line;
            } catch(Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching Line: ID {lineId}");
                return null;
            }
        }

        public async Task<IEnumerable<Line>> GetByLineStateAsync(LineState state)
        {
            try
            {
                var lines = await _lineRepository.GetByLineState(state);
                if(!lines.Any())
                {
                    _logger.LogWarning($"No lines found with state: {state}");
                }

                return lines;
            } catch(Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching Lines with state: {state}");
                return Enumerable.Empty<Line>();
            }
        }

        public async Task<bool> UpdateLineStateAsync(int lineId, LineState newState, int workerId)
        {
            try
            {
                var line = await _lineRepository.GetByIdAsync(lineId);
                if (line == null)
                {
                    _logger.LogWarning($"Line not found: ID {lineId}");
                    return false;
                }

                var prevState = line.CurrentState; // 변경 전 상태 저장
                line.CurrentState = newState;

                if (await _lineRepository.SaveChangesAsync()) // 변경사항 저장
                {
                    _logger.LogInformation(
                        "Line state updated: LineId {LineId}, WorkerId {WorkerId}, PrevState {PrevState}, NewState {NewState}, EventTime {EventTime}",
                        lineId,
                        workerId,
                        prevState,
                        newState,
                        DateTime.Now);

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating Line state: ID {lineId}, New State: {newState}");
                return false;
            }
        }
    }
}
