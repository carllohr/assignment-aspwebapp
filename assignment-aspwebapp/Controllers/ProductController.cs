using assignment_aspwebapp.Models.Entities;
using assignment_aspwebapp.Models.Forms;
using assignment_aspwebapp.Services;
using assignment_aspwebapp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace assignment_aspwebapp.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllAsync();
            return View(products);
        }

        [Authorize(Roles = "Administrator, Product Manager")]
        public IActionResult Create(string ReturnUrl = null!)
        {
            var form = new ProductForm { ReturnUrl = ReturnUrl ?? Url.Content("~/") };
            return View(form);
        }
        [Authorize(Roles = "Administrator, Product Manager")]
        [HttpPost]
        public async Task<IActionResult> Create(ProductForm form) // creates product by taking in productform values from input;
        {

            if (ModelState.IsValid && form.PicUrl != null)
            {
                await _productService.CreateAsync(form);
                return LocalRedirect("~/");
            }
            ModelState.AddModelError("", "Could not add product. Make sure all the required information is filled in and a picture is attached.");
            return View(form);
        }

        public async Task<IActionResult> Details(string id, string ReturnUrl = null!) // use Tuple to be able to pass two models in to one view;
        {
            var products = await _productService.GetAllAsync();
            var product = await _productService.GetByIdAsync(id);
            var tuple = new Tuple<IEnumerable<ProductEntity>, ProductEntity>(products, product);
            return View(tuple);
        }
        [Authorize(Roles = "Administrator, Product Manager")]
        public async Task<IActionResult> Delete(string id)
        {
           var result = await _productService.DeleteAsync(id);

            if (result)
            {
                return RedirectToAction("Index", "Product");
            }

            return new BadRequestResult();

        }
        [Authorize(Roles = "Administrator, Product Manager")]

        public async Task<IActionResult> EditProduct(string id)
        {
            var product = await _productService.GetProductDataAsync(id);
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductViewModel product)
        {
            if (ModelState.IsValid)
            {
               var result = await _productService.UpdateProductAsync(product);
                if(result is OkResult)
                {
                    return RedirectToAction("Index", "Product");
                }
                return new BadRequestResult();
            }

            return View(product);
        }
    }
}
