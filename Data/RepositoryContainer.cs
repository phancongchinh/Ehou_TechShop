using TechShop.Models.Entity;

namespace TechShop.Data
{
    public class RepositoryContainer
    {
        private AppDbContext _context;

        public readonly Repository<User> UserRepository;
        public Repository<Product> ProductRepository;
        public Repository<Category> CategoryRepository;
        public readonly Repository<Purchase> PurchaseRepository;
        public Repository<PurchaseProduct> PurchaseProductRepository;
        public readonly Repository<UserRole> UserRoleRepository;
        public readonly Repository<Image> ImageRepository;


        public RepositoryContainer()
        {
            _context = AppDbContext.Init();
            UserRepository = new Repository<User>(_context);
            ProductRepository = new Repository<Product>(_context);
            CategoryRepository = new Repository<Category>(_context);
            PurchaseRepository = new Repository<Purchase>(_context);
            PurchaseProductRepository = new Repository<PurchaseProduct>(_context);
            UserRoleRepository = new Repository<UserRole>(_context);
            ImageRepository = new Repository<Image>(_context);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}