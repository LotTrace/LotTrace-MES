using LotTrace_MES.src.Application.DTO.Request.Line;
using LotTrace_MES.src.Application.DTO.Response.Line;
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
        public LineService(ILineRepository lineRepository, ILogger<LineService> logger)
        {
            _lineRepository = lineRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<ResponseLineDTO>> GetAllLinesAsync()
        {
            try
            {
                var lines = await _lineRepository.GetAllAsync();
                return lines.Select(line => new ResponseLineDTO {
                    LineId = line.LineId,
                    LineName = line.LineName, 
                    Description = line.Description, 
                    CurrentState = line.CurrentState, 
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all Lines");
                throw;
            }
        }

        public async Task<ResponseLineDTO?> GetByLineIdAsync(int lineId)
        {
            try
            {
                var line = await _lineRepository.GetByIdAsync(lineId);
                if (line == null)
                {
                    _logger.LogWarning($"Line not found: ID {lineId}");
                    return null;
                }

                var response = new ResponseLineDTO
                {
                    LineId = line.LineId,
                    LineName = line.LineName,
                    Description = line.Description,
                    CurrentState = line.CurrentState,
                };

                return response;

            } catch(Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching Line: ID {lineId}");
                return null;
            }
        }

        public async Task<IEnumerable<ResponseLineDTO>> GetByLineStateAsync(LineState state)
        {
            try
            {
                var lines = await _lineRepository.GetByLineState(state);
                if(!lines.Any())
                {
                    _logger.LogWarning($"No lines found with state: {state}");
                }

                return lines.Select(line => new ResponseLineDTO
                {
                    LineId = line.LineId,
                    LineName = line.LineName,
                    Description = line.Description,
                    CurrentState = line.CurrentState,
                }).ToList();
            } catch(Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching Lines with state: {state}");
                return Enumerable.Empty<ResponseLineDTO>();
            }
        }

        public async Task<ResponseLineDTO> CreateLineAsync(CreateRequestLineDTO createRequestLineDTO)
        {
            try
            {
                var newLine = new Line
                {
                    LineName = createRequestLineDTO.LineName,
                    Description = createRequestLineDTO.Description,
                    CurrentState = createRequestLineDTO.CurrentState
                };

                await _lineRepository.AddAsync(newLine);
                await _lineRepository.SaveChangesAsync();

                var response = new ResponseLineDTO
                {
                    LineId = newLine.LineId,
                    LineName = newLine.LineName,
                    Description = newLine.Description,
                    CurrentState = newLine.CurrentState
                };

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new Line");
                throw;
            }
        }

        public async Task<bool> DeleteByIdAsync(int lineId)
        {
            try
            {

                var line = await _lineRepository.GetByIdAsync(lineId);

                if (line == null)
                {
                    _logger.LogWarning($"Line not found: ID {lineId}");
                    return false;
                }

                _lineRepository.Delete(line);
                await _lineRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting Line: ID {lineId}");
                return false;
            }
        }

        public async Task<bool> UpdateLineStateAsync(UpdateRequestLineDTO updateRequestLineDTO)
        {
            try
            {
                var line = await _lineRepository.GetByIdAsync(updateRequestLineDTO.LineId);
                if (line == null)
                {
                    _logger.LogWarning($"Line not found: ID {updateRequestLineDTO.LineId}");
                    return false;
                }

                var prevState = line.CurrentState; // 변경 전 상태 저장
                line.CurrentState = updateRequestLineDTO.NewState;

                if (await _lineRepository.SaveChangesAsync()) // 변경사항 저장
                {
                    _logger.LogInformation(
                        "Line state updated: LineId {LineId}, WorkerId {WorkerId}, PrevState {PrevState}, NewState {NewState}, EventTime {EventTime}",
                        updateRequestLineDTO.LineId,
                        updateRequestLineDTO.WorkerId,
                        prevState,
                        updateRequestLineDTO.NewState,
                        DateTime.Now);

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating Line state: ID {updateRequestLineDTO.LineId}, New State: {updateRequestLineDTO.NewState}");
                return false;
            }
        }
    }
}
