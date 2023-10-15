using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace TechShop.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        public DbSet<T> DbSet { get; }

        public Repository(AppDbContext context)
        {
            _context = context;
            DbSet = _context.Set<T>();
        }
        
        

        public IEnumerable<T> Get(Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = DbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                         (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return orderBy != null ? orderBy(query).ToList() : query.ToList();
        }

        public T? GetById(object id)
        {
            return DbSet.Find(id);
        }

        public void Insert(T entity)
        {
            DbSet.Add(entity);
        }

        public void Update(T entityToUpdate)
        {
            DbSet.Update(entityToUpdate);
        }

        public void Delete(object id)
        {
            var entityToDelete = DbSet.Find(id);
            if (entityToDelete != null) Delete(entityToDelete);
        }

        public void Delete(T entityToDelete)
        {
            // Detached means the entity exists in the application but not managed by entityManager
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }

            DbSet.Remove(entityToDelete);
        }
    }
}