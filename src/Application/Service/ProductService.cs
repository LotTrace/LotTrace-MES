using LotTrace_MES.src.Domain.Interfaces;
using LotTrace_MES.src.Application.DTO.Request.Product;
using LotTrace_MES.src.Application.Interfaces;
using LotTrace_MES.src.Domain.Entity;

namespace LotTrace_MES.src.Application.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IProductRepository productRepository, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }
        public async Task<Product?> CreateProductAsync(CreateRequestProductDTO createRequestDTO)
        {
            try
            {
                var existingProduct = await _productRepository.GetProductCodeAsync(createRequestDTO.ProductCode);
                if (existingProduct != null) // 이미 존재하는 제품 코드인 경우 null 반환
                {
                    _logger.LogWarning($"Existing Product Code : {createRequestDTO.ProductCode}");
                    return null;
                }

                var product = new Product
                {
                    ProductCode = createRequestDTO.ProductCode,
                    ProductName = createRequestDTO.ProductName
                };

                await _productRepository.AddAsync(product);
                await _productRepository.SaveChangesAsync();

                return product;

            } catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while creating product with code {createRequestDTO.ProductCode}");
                return null;
            }
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            try
            {
                return await _productRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all products");
                return Enumerable.Empty<Product>();
            }
        }


        public async Task<Product?> GetProductByCodeAsync(string productCode)
        {
            try
            {
                var product = await _productRepository.GetProductCodeAsync(productCode);
                if(product == null)
                {
                    _logger.LogWarning($"Product with code {productCode} not found");
                    return null;
                }

                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving product with code {productCode}");
                return null;
            }
        }
    }
}
