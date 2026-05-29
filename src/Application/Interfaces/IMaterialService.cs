using LotTrace_MES.src.Application.DTO.Response.Material;
using LotTrace_MES.src.Domain.Entity;

namespace LotTrace_MES.src.Domain.Services
{
    public interface IMaterialService
    {
        Task<bool> InboundMaterialAsync(int materialId, int quantity);

        Task<bool> DeductStockAsync(int materialId, int quantity);

        Task<bool> AdjustStockAsync(int materialId, int newStockQty);

        Task<IEnumerable<ResponseMaterialDTO>> GetDangerousStockMaterialsAsync();

        Task<IEnumerable<ResponseMaterialDTO>> GetAllMaterialsAsync();
        Task<ResponseMaterialDTO> GetMaterialByIdAsync(int materialId);
    }
}