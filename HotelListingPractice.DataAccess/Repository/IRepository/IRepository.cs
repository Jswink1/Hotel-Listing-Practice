using HotelListingPractice.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace HotelListingPractice.DataAccess.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<IList<T>> GetAll(Expression<Func<T, bool>> expression = null,
                              Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                              List<string> includes = null);

        Task<IPagedList<T>> GetAll(RequestParams requestParams, List<string> includes = null);

        Task<T> Get(Expression<Func<T, bool>> expression,
                    List<string> includes = null);

        Task Insert(T entity);
        Task InsertRange(IEnumerable<T> entities);
        void Delete(int id);
        void DeleteRange(IEnumerable<T> entities);
        void Update(T entity);
    }
}
