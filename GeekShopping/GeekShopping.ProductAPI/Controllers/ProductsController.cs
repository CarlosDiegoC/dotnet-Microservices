using GeekShopping.ProductAPI.Data.ValueObjects;
using GeekShopping.ProductAPI.Repository;
using GeekShopping.ProductAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.ProductAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductRepository _productRepository;
        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new 
                ArgumentNullException(nameof(productRepository));
        }
        
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ProductVO>>> FindAll()
        {          
            var productsVO = await _productRepository.FindAll();
            return Ok(productsVO);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ProductVO>> FindById(long id)
        {
            ProductVO productVO = await _productRepository.FindById(id);
            if (productVO == null) return BadRequest("Product not found.");
            return Ok(productVO);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ProductVO>> Create(ProductVO productVO)
        {
            if (productVO == null) return BadRequest();
            await _productRepository.Create(productVO);
            return Ok(productVO);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<ProductVO>> Update(ProductVO productVO)
        {
            if (productVO == null) return BadRequest();
            await _productRepository.Update(productVO);
            return Ok(productVO);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<ActionResult<bool>> Delete(long id)
        {
            var status = await _productRepository.Delete(id);
            if(!status) return BadRequest(status);
            return Ok(status);
        }
    }
}
