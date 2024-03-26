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
        //_context.Products.Update(product);
        var productFromDb = _context.Products.FirstOrDefault(p => p.Id == product.Id);

        if (productFromDb != null)
        {
            productFromDb.Title = product.Title;
            productFromDb.Description = product.Description;
            productFromDb.ListPrice = product.ListPrice;
            productFromDb.Price = product.Price;
            productFromDb.Price50 = product.Price50;
            productFromDb.Price100 = product.Price100;
            productFromDb.Description = product.Description;
            productFromDb.CategoryId = product.CategoryId;
            productFromDb.Author = product.Author;

            if (productFromDb.ImageUrl != null) 
            {
                productFromDb.ImageUrl = product.ImageUrl;
            }
        }
    }
}
