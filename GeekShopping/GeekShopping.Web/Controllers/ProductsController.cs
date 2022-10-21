using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;
using GeekShopping.Web.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        [Authorize]
        public async Task<ActionResult> ProductsIndex()
        {
            var products = await _productService.FindAllProducts();
            return View(products);
        }
        
        public async Task<ActionResult> ProductsCreate()
        {   
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> ProductsCreate(ProductModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.CreateProduct(model);
                if(response != null) return RedirectToAction(nameof(ProductsIndex));
            }
            
            return View(model);
        }

        public async Task<ActionResult> ProductsUpdate(int id)
        {
            var model = await _productService.FindProductById(id);
            if(model != null) return View(model);
            return NotFound();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> ProductsUpdate(ProductModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.UpdateProduct(model);
                if (response != null) return RedirectToAction(nameof(ProductsIndex));
            }

            return View(model);
        }

        [Authorize]
        public async Task<ActionResult> ProductsDelete(int id)
        {
            var model = await _productService.FindProductById(id);
            if (model != null) return View(model);
            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = Role.Admin)]        
        public async Task<ActionResult> ProductsDelete(ProductModel model)
        {
            var response = await _productService.DeleteProductById(model.Id);
            if (response) return RedirectToAction(nameof(ProductsIndex));      
            return View(model);
        }
    }
}
