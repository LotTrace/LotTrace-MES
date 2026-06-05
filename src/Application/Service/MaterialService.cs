using LotTrace_MES.src.Application.DTO.Response.Material;
using LotTrace_MES.src.Domain.Interfaces;
using LotTrace_MES.src.Domain.Services;

namespace LotTrace_MES.src.Application.Service
{
    public class MaterialService : IMaterialService
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly ILogger<MaterialService> _logger;

        public MaterialService(IMaterialRepository materialRepository, ILogger<MaterialService> logger)
        {
            _materialRepository = materialRepository;
            _logger = logger;
        }

        public async Task<bool> AdjustStockAsync(int materialId, int newStockQty) // 관리자용 강제 수정
        {
            try
            {
                var material = await _materialRepository.GetByIdAsync(materialId);
                if (material == null)
                {
                    _logger.LogWarning($"Material ID {materialId} not found for stock adjustment.");
                    return false;
                }

                material.Stock = newStockQty;
                await _materialRepository.SaveChangesAsync();

                _logger.LogInformation($"Material ID {materialId} stock adjusted to {newStockQty}.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while adjusting stock for Material ID {materialId} to {newStockQty}.");
                return false;
            }
        }

        public async Task<bool> DeductStockAsync(int materialId, int quantity)
        {
            try
            {
                var material = await _materialRepository.GetByIdAsync(materialId);
                if (material == null)
                {
                    _logger.LogWarning($"[Deduct] Material ID {materialId} not found.");
                    return false;
                }
                if (material.Stock < quantity)
                {
                    _logger.LogWarning($"[Deduct] Material {material.Name}(ID: {materialId}) shortage! Requested: {quantity}, Available: {material.Stock}");
                    throw new InvalidOperationException($"자재 재고가 부족하여 공정에 투입할 수 없습니다. (부족 수량: {quantity - material.Stock}개)");
                }

                material.Stock -= quantity;
                await _materialRepository.SaveChangesAsync();

                _logger.LogInformation($"[Deduct] Material {material.Name}(ID: {materialId}) deducted by {quantity}. Remaining Stock: {material.Stock}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deducting stock for Material ID {materialId}");
                throw;
            }
        }

        public async Task<IEnumerable<ResponseMaterialDTO>> GetAllMaterialsAsync()
        {
            try
            {
                var materials = await _materialRepository.GetAllAsync();
                return materials.Select(m => new ResponseMaterialDTO
                {
                    MaterialId = m.MaterialId,
                    Name = m.Name,
                    Stock = m.Stock,
                    IsLowStock = m.Stock <= 10 // 예시로 10개 이하를 저재고로 간주
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all materials");
                return Enumerable.Empty<ResponseMaterialDTO>();
            }
        }

        public async Task<IEnumerable<ResponseMaterialDTO>> GetDangerousStockMaterialsAsync()
        {
            try
            {
                var lowStockMaterials = await _materialRepository.GetLowStockMaterialAsync();

                return lowStockMaterials.Select(m => new ResponseMaterialDTO
                {
                    MaterialId = m.MaterialId,
                    Name = m.Name,
                    Stock = m.Stock,
                    IsLowStock = true
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving dangerous stock materials");
                throw;
            }
        }

        public async Task<ResponseMaterialDTO> GetMaterialByIdAsync(int materialId)
        {
            try
            {
                var material = await _materialRepository.GetByIdAsync(materialId);

                if (material == null)
                {
                    _logger.LogWarning($"Material ID {materialId} not found.");
                    throw new KeyNotFoundException($"Material with ID {materialId} not found.");
                }

                return new ResponseMaterialDTO
                {
                    Name = material.Name,
                    Stock = material.Stock,
                    MaterialId = material.MaterialId,
                    IsLowStock = material.Stock <= 10
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving material by ID {materialId}");
                throw;
            }
        }

        public async Task<bool> InboundMaterialAsync(int materialId, int quantity)
        {
            try
            {
                var material = await _materialRepository.GetByIdAsync(materialId);
                if (material == null)
                {
                    _logger.LogWarning($"Material ID {materialId} not found for inbound operation.");
                    return false;
                }

                material.Stock += quantity;
                await _materialRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while processing inbound for Material ID {materialId} with quantity {quantity}.");
                return false;
            }
        }
    }
}
