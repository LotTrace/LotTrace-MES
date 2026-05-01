using LotTrace_MES.src.Application.DTO.Request.Product;
using LotTrace_MES.src.Application.DTO.Response.Product;
using LotTrace_MES.src.Application.Interfaces;
using LotTrace_MES.src.Domain.Entity;
using LotTrace_MES.src.Domain.Interfaces;

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
        public async Task<ResponseProductDTO?> CreateProductAsync(RequestProductDTO createRequestDTO)
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

                var response = new ResponseProductDTO
                {
                    ProductId = product.ProductId,
                    ProductCode = product.ProductCode ?? "NoCode",
                    ProductName = product.ProductName ?? "Unknown",
                };
                return response;

            } catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while creating product with code {createRequestDTO.ProductCode}");
                return null;
            }
        }

        public async Task<bool> UpdateProduct(int productId, RequestProductDTO RequestDTO)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null)
                {
                    _logger.LogWarning($"Product with ID {productId} not found for update");
                    return false;
                }
                product.ProductName = RequestDTO.ProductName ?? product.ProductName;
                product.ProductCode = RequestDTO.ProductCode ?? product.ProductCode;

                _productRepository.Updated(product);
                await _productRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating product with ID {productId}");
                return false;
            }
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null)
                {
                    _logger.LogWarning($"Product with ID {productId} not found for deletion");
                    return false;
                }

                _productRepository.Delete(product);
                await _productRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting product with ID {productId}");
                return false;
            }
        }

        public async Task<IEnumerable<ResponseProductDTO>> GetAllProductsAsync()
        {
            try
            {
                var products = await _productRepository.GetAllAsync();

                if (products == null || !products.Any())
                {
                    _logger.LogWarning("No products found");
                    return Enumerable.Empty<ResponseProductDTO>();
                }

                return products.Select(product => new ResponseProductDTO
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName ?? "Unknown", 
                    ProductCode = product.ProductCode ?? "NoCode"  
                }).ToList(); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all products");
                return Enumerable.Empty<ResponseProductDTO>();
            }
        }


        public async Task<ResponseProductDTO?> GetProductByCodeAsync(string productCode)
        {
            try
            {
                var product = await _productRepository.GetProductCodeAsync(productCode);
                if(product == null)
                {
                    _logger.LogWarning($"Product with code {productCode} not found");
                    return null;
                }

                var response = new ResponseProductDTO
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName ?? "Unknown",
                    ProductCode = product.ProductCode ?? "NoCode"
                };
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving product with code {productCode}");
                return null;
            }
        }

        public async Task<ResponseProductDTO?> GetProductByIdAsync(int productId)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null)
                {
                    _logger.LogWarning($"Product with ID {productId} not found");
                    return null;
                }

                var response = new ResponseProductDTO
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName ?? "Unknown",
                    ProductCode = product.ProductCode ?? "NoCode"
                };
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving product with ID {productId}");
                return null;
            }
        }
    }
}
