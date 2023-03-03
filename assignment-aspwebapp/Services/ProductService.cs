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
