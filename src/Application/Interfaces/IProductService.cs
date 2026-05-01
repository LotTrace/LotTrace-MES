using LotTrace_MES.src.Application.DTO.Request.Product;
using LotTrace_MES.src.Application.DTO.Response.Product;

namespace LotTrace_MES.src.Application.Interfaces
{
    public interface IProductService
    {
        Task<ResponseProductDTO?> GetProductByCodeAsync(string productCode); // 제품코드로 제품 조회
        Task<ResponseProductDTO?> GetProductByIdAsync(int productId); // 제품 ID로 제품 조회
        Task<IEnumerable<ResponseProductDTO>> GetAllProductsAsync(); // 모든 제품 조회
        Task<ResponseProductDTO?> CreateProductAsync(RequestProductDTO createRequestDTO); // 제품 생성
        Task<bool> DeleteProduct(int productId); // 제품 삭제
        Task<bool> UpdateProduct(int productId, RequestProductDTO requestDTO); // 제품 업데이트
    }
}
