namespace assignment_aspwebapp.ViewModels
{
    public class ProductViewModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = null!;
        public string ShortDescription { get; set; } = null!;
        public string LongDescription { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public string? ImageName { get; set; }
        public string? ImageAlt { get; set; }
        public string Category { get; set; } = null!;
        public IFormFile? PicUrl { get; set; }
        public string? Tag { get; set; }
    }
}
