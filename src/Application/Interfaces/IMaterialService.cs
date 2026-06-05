using LotTrace_MES.src.Application.DTO.Response.Material;

namespace LotTrace_MES.src.Application.Interfaces
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