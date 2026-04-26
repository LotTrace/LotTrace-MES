using LotTrace_MES.src.Domain.Entity;

namespace LotTrace_MES.src.Application.Interfaces
{
    public interface IProductService
    {
        Task<Product?> GetProductCodeAsync(string productCode); // 제품코드로 제품 조회
        Task<IEnumerable<Product>> GetAllProductsAsync(); // 모든 제품 조회
        Task<Product?> CreateProductAsync(string productCode, string productName); // 제품 생성
    }
}
