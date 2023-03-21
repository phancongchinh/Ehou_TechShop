using TechShop.Models.Entity;

namespace TechShop.Data
{
    public class UnitOfWork
    {
        private AppDbContext _context;

        public readonly Repository<User> UserRepository;
        public readonly Repository<Product> ProductRepository;
        public readonly Repository<Category> CategoryRepository;
        public readonly Repository<Purchase> PurchaseRepository;
        public readonly Repository<PurchaseProduct> PurchaseProductRepository;
        public readonly Repository<ShoppingCartItem> ShoppingCartItemRepository;
        public readonly Repository<UserRole> UserRoleRepository;
        public readonly Repository<Image> ImageRepository;

        public UnitOfWork()
        {
            _context = AppDbContext.Init();
            UserRepository = new Repository<User>(_context);
            ProductRepository = new Repository<Product>(_context);
            CategoryRepository = new Repository<Category>(_context);
            PurchaseRepository = new Repository<Purchase>(_context);
            PurchaseProductRepository = new Repository<PurchaseProduct>(_context);
            ShoppingCartItemRepository = new Repository<ShoppingCartItem>(_context);
            UserRoleRepository = new Repository<UserRole>(_context);
            ImageRepository = new Repository<Image>(_context);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}