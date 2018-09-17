using Manage.Core.Data;
using Manage.Core.Pageing;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Expressions;

namespace Manage.Core.Infrastructure
{
    /// <summary>
    /// EF仓库
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : class, new()
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="dbContext">EF上下文</param>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        int Insert(DbContext dbContext, T entity);

        int Insert(DbContext dbContext, List<T> entities);

        int Delete(DbContext dbContext, Expression<Func<T, bool>> where);

        int Update(DbContext dbContext, T entity);

        T Entity(DbContext dbContext, Expression<Func<T, bool>> where);

        List<T> Entities(DbContext dbContext, Expression<Func<T, bool>> where);

        List<T> Entities(DbContext dbContext, Expression<Func<T, bool>> where, OrderModelField[] orderByExpression);

        Page<T> FindPage(DbContext dbContext, Expression<Func<T, bool>> where, OrderModelField[] orderByExpression, int pageIndex, int pageSize);

        int ExecuteSqlCommand(DbContext dbContext, string sql, params object[] parameters);

        List<T> Entities(DbContext dbContext, string sql, params object[] parameters);

        Page<K> FindPage<K>(DbContext dbContext, Pageination page);
    }
}
