using assignment_aspwebapp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace assignment_aspwebapp.Contexts
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<ProductEntity> Products { get; set; } = null!;
    }
}
