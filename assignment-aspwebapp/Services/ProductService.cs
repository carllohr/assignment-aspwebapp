using assignment_aspwebapp.Contexts;
using assignment_aspwebapp.Models.Entities;
using assignment_aspwebapp.Models.Forms;
using assignment_aspwebapp.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace assignment_aspwebapp.Services
{
    public class ProductService
    {
        private readonly DataContext _context;
        private readonly UserService _userService;

        public ProductService(DataContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<List<ProductEntity>> GetAllAsync()
        {
            try
            {
                var products = await _context.Products.ToListAsync();
                return products;
            }
            catch { return null!; }
        }
        public async Task<ProductEntity> GetByIdAsync(string id)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.Id.ToString() == id);
                return product!;
            }
            catch { return null!; }
        }

        public async Task<ProductViewModel> GetProductDataAsync(string id)
        {
            var productEntity = await _context.Products.FirstOrDefaultAsync(x => x.Id.ToString() == id);
            if (productEntity != null) 
            {
                var product = new ProductViewModel
                {
                    Id = productEntity.Id,
                    Name = productEntity.Name,
                    ShortDescription = productEntity.ShortDescription,
                    LongDescription = productEntity.LongDescription,
                    Price = productEntity.Price,
                    DiscountPrice = productEntity.DiscountPrice,
                    ImageAlt = productEntity.ImageAlt,
                    ImageName = productEntity.ImageName,
                    Category = productEntity.Category,
                    Tag = productEntity.Tag,
                };

                return product;
            }
            return null!;
        }

        public async Task<IActionResult> UpdateProductAsync(ProductViewModel product)
        {
            var productEntity = await _context.Products.FirstOrDefaultAsync(x => x.Id == product.Id); 

            if (productEntity != null)
            {
                productEntity.Name = product.Name;
                productEntity.ShortDescription = product.ShortDescription;
                productEntity.LongDescription = product.LongDescription;
                productEntity.Price = product.Price;
                productEntity.DiscountPrice = (decimal)product.DiscountPrice!;
                productEntity.Tag = product.Tag;
                productEntity.ImageAlt= product.ImageAlt;

                if (product.PicUrl != null)
                {
                    productEntity.ImageName = await _userService.UploadProfileImageAsync(product.PicUrl);
                }

                _context.Entry(productEntity).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return new OkResult();
            }

            return new BadRequestResult();
            

           
        }

        public async Task<bool> DeleteAsync(string id)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.Id.ToString() == id);
                _context.Products.Remove(product!);
                await _context.SaveChangesAsync();
                return true;
            }
            catch { return false; }

        }
    
        public async Task<IActionResult> CreateAsync(ProductForm form)
        {
            if (form != null)
            {
                var productEntity = new ProductEntity
                {
                    Name = form.Name,
                    ShortDescription = form.ShortDescription,
                    LongDescription = form.LongDescription,
                    Price = form.Price,
                    DiscountPrice = (decimal)form.DiscountPrice!,
                    Category = form.Category,
                    Tag = form.Tag,
                    ImageAlt = form.ImageAlt,
                };
                if (form.PicUrl != null)
                {
                    productEntity.ImageName = await _userService.UploadProfileImageAsync(form.PicUrl);
                }

                await _context.AddAsync(productEntity);
                await _context.SaveChangesAsync();

                return new OkResult();
                
                
            }

            return new BadRequestResult();

        }
    }
}
