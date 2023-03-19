using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace TechShop.Data
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> Get(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>,
                IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");

        T? GetById(object id);

        void Insert(T entity);

        void Update(T entityToUpdate);

        void Delete(object id);

        void Delete(T entityToDelete);
    }
}