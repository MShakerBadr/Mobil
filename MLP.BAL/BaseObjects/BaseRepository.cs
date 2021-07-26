using MLP.BAL.BaseObjects.ReporistoriesInterfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MLP.BAL.BaseObjects
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        #region Properties
        protected DbContext DbContext { get; set; }

        protected DbSet<T> DbSet { get; set; }

        #endregion




        public BaseRepository(DbContext dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException("dbContext");

            DbContext = dbContext;
            DbSet = DbContext.Set<T>();
        }

        #region Get All Data Methods
        public virtual IQueryable<T> GetAll()
        {
            return DbSet;
        }

        /// <summary>
        /// Get All data sorted by lambda expression
        /// </summary>
        /// <typeparam name="TKey">Type of sorting key or property</typeparam>
        /// <param name="sortingExpression">lambda expression for sorting ex: T => T.Key</param>
        /// <returns>IOrderedQueryable<typeparamref name="T"/></returns>
        public IQueryable<T> GetAllSorted<TKey>(Expression<Func<T, TKey>> sortingExpression)
        {
            return DbSet.OrderBy<T, TKey>(sortingExpression);
        }

        public IQueryable<T> GetWhere(System.Linq.Expressions.Expression<Func<T, bool>> filter = null, string includeProperties = "")
        {
            IQueryable<T> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return query;
        }
        #endregion

        #region Get one record
        public virtual T GetById(int id)
        {
            //return DbSet.FirstOrDefault(PredicateBuilder.GetByIdPredicate<T>(id));
            return DbSet.Find(id);
        }

        public virtual T GetById(long id)
        {
            //return DbSet.FirstOrDefault(PredicateBuilder.GetByIdPredicate<T>(id));
            return DbSet.Find(id);
        }

        #endregion

        #region CRUD Methods
        public virtual bool Insert(T entity)
        {
            bool returnVal = false;
            try
            {
                DbEntityEntry dbEntityEntry = DbContext.Entry(entity);
                if (dbEntityEntry.State != EntityState.Detached)
                {
                    dbEntityEntry.State = EntityState.Added;
                }
                else
                {
                    DbSet.Add(entity);
                }
                returnVal = true;
            }
            catch (Exception)
            {

                throw;
            }
            return returnVal;
        }

        public virtual void InsertList(List<T> entityList)
        {
            foreach (T item in entityList)
            {
                Insert(item);
            }
        }

        public void InsertListAsBulk(List<T> entityList)
        {
            DbSet.AddRange(entityList);
        }
        //public virtual void Update(T entityToUpdate)
        //{
        //    var entry = this.DbContext.Entry(entityToUpdate);
        //    var key = this.GetPrimaryKey(entry);

        //    if (entry.State == EntityState.Detached)
        //    {
        //        var currentEntry = this.DbSet.Find(key);
        //        if (currentEntry != null)
        //        {
        //            var attachedEntry = this.DbContext.Entry(currentEntry);
        //            attachedEntry.CurrentValues.SetValues(entityToUpdate);
        //        }
        //        else
        //        {
        //            this.DbSet.Attach(entityToUpdate);
        //            entry.State = EntityState.Modified;
        //        }
        //    }
        //    else
        //    {
        //        this.DbSet.Attach(entityToUpdate);
        //        entry.State = EntityState.Modified;
        //    }
        //}

        //private object GetPrimaryKey(DbEntityEntry entry)
        //{
        //    var myObject = entry.Entity;
        //    var property =
        //        myObject.GetType()
        //            .GetProperties()
        //            .FirstOrDefault(prop => prop.Name == "ID");
        //    if (property.PropertyType.Name == "Guid")
        //    {
        //        return (Guid)property.GetValue(myObject, null);
        //    }
        //    else
        //    {
        //        return (int)property.GetValue(myObject, null);
        //    }
        //}
        public virtual void Update(T entity)
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry(entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            dbEntityEntry.State = EntityState.Modified;
        }
        public virtual void UpdateList(List<T> entityList)
        {
            foreach (T item in entityList)
            {
                Update(item);
            }
        }

        public virtual void Delete(T entity)
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry(entity);
            if (dbEntityEntry.State != EntityState.Deleted)
            {
                dbEntityEntry.State = EntityState.Deleted;
            }
            else
            {
                DbSet.Attach(entity);
                DbSet.Remove(entity);
            }
        }
        public virtual void Delete(int id)
        {
            var entity = GetById(id);
            if (entity == null) return; // not found; assume already deleted.
            Delete(entity);
        }

        #endregion
        #region Get Data in Range - Take - Skip

        /// <summary>
        /// Get range of data sorted by lambda expression
        /// </summary>
        /// <typeparam name="TKey">Type of sorting key or property</typeparam>
        /// <param name="skip">number of records to be skipped </param>
        /// <param name="take">number of records to be taken</param>
        /// <param name="sortingExpression">lambda expression for sorting ex: T => T.Key</param>
        /// <param name="sortDir">Sorting direction </param>
        /// <returns>IQueryable<T></returns>
        public virtual IQueryable<T> GetPage<TKey>(int skipCount, int takeCount, Expression<Func<T, TKey>> sortingExpression)
        {
            IQueryable<T> queryResult = null;

            if (skipCount == 0)
                queryResult = DbSet.OrderBy<T, TKey>(sortingExpression).Take(takeCount);
            else
                queryResult = DbSet.OrderBy<T, TKey>(sortingExpression).Skip(skipCount).Take(takeCount);

            return queryResult;

        }

        /// <summary>
        /// Get range of filtered data sorted by lambda expression
        /// </summary>
        /// <typeparam name="TKey">Type of sorting key or property</typeparam>
        /// <param name="skip">number of records to be skipped </param>
        /// <param name="take">number of records to be taken</param>
        /// <param name="sortingExpression">lambda expression for sorting ex: T => T.Key</param>
        /// <param name="filter">lambda expression for filtering ex: T => T.Key == value</param>
        /// <param name="sortDir">Sorting direction </param>
        /// <param name="includeString">navigation properties separated by comma to be included</param>
        /// <returns>IQueryable<T></returns>
        //public virtual IQueryable<T> GetPageWhere<TKey>(int skipCount, int takeCount, Expression<Func<T, TKey>> sortingExpression, Expression<Func<T, bool>> filter, SortDirection sortDir, string includeString)
        //{
        //    IQueryable<T> queryResult = null;

        //    switch (sortDir)
        //    {
        //        case SortDirection.Ascending:
        //            if (skipCount == 0)
        //                queryResult = GetWhere(filter, includeString).OrderBy<T, TKey>(sortingExpression).Take(takeCount);
        //            else
        //                queryResult = GetWhere(filter, includeString).OrderBy<T, TKey>(sortingExpression).Skip(skipCount).Take(takeCount);
        //            break;
        //        case SortDirection.Descending:
        //            if (skipCount == 0)
        //                queryResult = GetWhere(filter, includeString).OrderByDescending<T, TKey>(sortingExpression).Take(takeCount);
        //            else
        //                queryResult = GetWhere(filter, includeString).OrderByDescending<T, TKey>(sortingExpression).Skip(skipCount).Take(takeCount);
        //            break;
        //        default:
        //            break;
        //    }
        //    return queryResult;
        //}
        /// <summary>
        /// Get All Data But By Sorting Direction.
        /// </summary>
        /// <typeparam name="TKey">Type Of Sorting</typeparam>
        /// <param name="sortingExpression"> Lambad Expression For Sorting</param>
        /// <param name="filter"> lambda Expreesioon For Filter </param>
        /// <param name="sortDir"> Sorting Asc or Des</param>
        /// <returns></returns>
        //public virtual IQueryable<T> GetAllOrderBy<TKey>(Expression<Func<T, TKey>> sortingExpression, Expression<Func<T, bool>> filter)
        //{
        //    IQueryable<T> queryResult = null;

        //    switch (sortDir)
        //    {
        //        case SortDirection.Ascending:
        //            queryResult = GetWhere(filter).OrderBy<T, TKey>(sortingExpression);
        //            break;
        //        case SortDirection.Descending:
        //            queryResult = GetWhere(filter).OrderByDescending<T, TKey>(sortingExpression);
        //            break;
        //        default:
        //            break;
        //    }
        //    return queryResult;
        //}

        #region Commented method
        ///// <summary>
        ///// Get range of filtered data sorted by lambda expression
        ///// </summary>
        ///// <typeparam name="TKey">Type of sorting key or property</typeparam>
        ///// <param name="skip">number of records to be skipped </param>
        ///// <param name="take">number of records to be taken</param>
        ///// <param name="sortingColumnName">column name for sorting ex: T => T.Key</param>
        ///// <param name="filter">lambda expression for filtering ex: T => T.Key == value</param>
        ///// <returns>IQueryable<T></returns>
        //public virtual IQueryable<T> GetPageWhere(int skipCount, int takeCount, string sortingColumnName, Expression<Func<T, bool>> filter, SortDirection sortDir, string includeString)
        //{
        //    IQueryable<T> queryResult = null;
        //    //PropertyInfo property = typeof(T).GetProperty(sortingColumnName);
        //    switch (sortDir)
        //    {

        //        case SortDirection.Ascending:
        //            if (skipCount == 0)
        //                queryResult = GetWhere(filter, includeString).OrderBy(sortingColumnName).Take(takeCount);
        //            else
        //                queryResult = GetWhere(filter, includeString).OrderBy(sortingColumnName).Skip(skipCount).Take(skipCount);
        //            break;
        //        case SortDirection.Descending:
        //            if (skipCount == 0)
        //                queryResult = GetWhere(filter, includeString).OrderByDescending(sortingColumnName).Take(takeCount);
        //            else
        //                queryResult = GetWhere(filter, includeString).OrderByDescending(sortingColumnName).Skip(skipCount).Take(skipCount);
        //            break;
        //        default:
        //            break;
        //    }
        //    return queryResult;
        //}

        #endregion

        #endregion

        #region Get Count Methods
        /// <summary>
        /// Get the count of rows
        /// </summary>
        /// <returns></returns>
        public virtual int GetCount()
        {
            return DbSet.Count();
        }

        /// <summary>
        /// Get the count of filtered rows
        /// </summary>
        /// <returns></returns>
        public virtual int GetCount(Expression<Func<T, bool>> filter)
        {
            return GetWhere(filter).Count();
        }

        #endregion

        public IEnumerable<T> SelectCustom(string query)
        {
            var data = this.DbContext.Database.SqlQuery<T>(query);

            return data;
        }

    }
}
