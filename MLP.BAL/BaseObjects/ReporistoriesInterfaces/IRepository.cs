using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MLP.BAL.BaseObjects.ReporistoriesInterfaces
{
    public interface IRepository<T> where T : class
    {

        IQueryable<T> GetAll();

        IQueryable<T> GetAllSorted<TKey>(Expression<Func<T, TKey>> sortingExpression);

        IQueryable<T> GetWhere(Expression<Func<T, bool>> filter = null, string includeProperties = "");

        T GetById(int entityId);

        bool Insert(T entity);
        void InsertList(List<T> entity);
        void InsertListAsBulk(List<T> entity);

        void Update(T entity);
        void UpdateList(List<T> entity);

        void Delete(T entity);

        void Delete(int entityId);


        IQueryable<T> GetPage<TKey>(int skipCount, int takeCount, Expression<Func<T, TKey>> sortingExpression);
    }
}
