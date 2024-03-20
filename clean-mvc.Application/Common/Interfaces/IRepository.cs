using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace clean_mvc.Application.Common.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAlls(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        T Get(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        void Insert(T entity);
        bool Any(Expression<Func<T, bool>> filter);
        void Delete(T entity);
    }
}
