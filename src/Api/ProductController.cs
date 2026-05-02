using LotTrace_MES.src.Application.DTO.Request.Product;
using LotTrace_MES.src.Application.DTO.Response.Product;
using LotTrace_MES.src.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LotTrace_MES.src.Api
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("code/{productCode}")]
        public async Task<ActionResult<ResponseProductDTO?>> GetProductByCodeAsync(string productCode)
        {
            var product = await _productService.GetProductByCodeAsync(productCode);
            if(product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<ResponseProductDTO?>> GetProductByIdAsync(int productId)
        {
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResponseProductDTO?>>> GetAllProductsAsync()
        {
            var products = await _productService.GetAllProductsAsync();
            if(products == null)
            {
                return NotFound();
            }

            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseProductDTO>> CreateProductAsync([FromBody] RequestProductDTO createRequestDTO)
        {
            var product = await _productService.CreateProductAsync(createRequestDTO);
            if(product == null)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetProductByCodeAsync), new { productCode = product.ProductCode }, product); 
        }

        [HttpDelete("{productId}")]
        public async Task<ActionResult<bool>> DeleteProduct(int productId)
        {
            var success = await _productService.DeleteProduct(productId);
            if (success == false)
            {
                return BadRequest();
            }

            return Ok(new { message = "Deleted successfully" });
        }

        [HttpPut("{productId}")]
        public async Task<ActionResult<bool>> UpdateProduct(int productId, [FromBody] RequestProductDTO requestDTO)
        {
            var success = await _productService.UpdateProduct(productId, requestDTO);
            if (!success)
            {
                return BadRequest();
            }

            return Ok(new { message = "Updated successfully" });
        }
    }
}
