using WebShop.DataAccess.Data;
using WebShop.DataAccess.Repository.IRepository;

namespace WebShop.DataAccess.Repository;

public class UnitOfWork : IUnitOfWork
{
    private ApplicationDbContext _context;
    public ICategoryRepository CategoryRepository { get; private set; }
    public IProductRepository ProductRepository { get; private set; }
    
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        CategoryRepository = new CategoryRepository(_context);
        ProductRepository = new ProductRepository(_context);
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}
