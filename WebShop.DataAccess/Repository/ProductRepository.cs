using WebShop.DataAccess.Data;
using WebShop.DataAccess.Repository.IRepository;
using WebShop.Models.Models;

namespace WebShop.DataAccess.Repository;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Product product)
    {
        _context.Products.Update(product);
    }
}
