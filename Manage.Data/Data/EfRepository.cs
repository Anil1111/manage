using Manage.Core.Data;
using Manage.Core.Infrastructure;
using Manage.Core.Pageing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace Manage.Data.Data
{
    public class EfRepository<T> : IRepository<T> where T : class, new()
    {
        public int Insert(DbContext dbContext, T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            EntityState state = dbContext.Entry(entity).State;
            if (state == EntityState.Detached)
            {
                dbContext.Entry(entity).State = EntityState.Added;
            }
            return dbContext.SaveChanges();
        }

        public int Insert(DbContext dbContext, List<T> entities)
        {
            if (entities.Count == 0)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            for (int i = 0; i < entities.Count; i++)
            {
                EntityState state = dbContext.Entry(entities[i]).State;
                if (state == EntityState.Detached)
                {
                    dbContext.Entry(entities[i]).State = EntityState.Added;
                }
            }
            return dbContext.SaveChanges();
        }

        public int Delete(DbContext dbContext, Expression<Func<T, bool>> where)
        {
            List<T> entities = dbContext.Set<T>().Where(where).ToList();
            try
            {
                dbContext.Configuration.AutoDetectChangesEnabled = false;
                foreach (T entity in entities)
                {
                    dbContext.Set<T>().Attach(entity);
                    dbContext.Entry<T>(entity).State = EntityState.Deleted;
                }
                return dbContext.SaveChanges();
            }
            finally
            {
                dbContext.Configuration.AutoDetectChangesEnabled = true;
            }
        }

        public int Update(DbContext dbContext, T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Exists(dbContext, entity);
            dbContext.Set<T>().Attach(entity);
            dbContext.Entry<T>(entity).State = EntityState.Modified;

            return dbContext.SaveChanges();
        }

        /// <summary>
        /// 如果上下文中存在对象则移除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private bool Exists(DbContext dbContext, T entity)
        {
            ObjectContext _ObjContext = ((IObjectContextAdapter)dbContext).ObjectContext;
            ObjectSet<T> _ObjSet = _ObjContext.CreateObjectSet<T>();
            var entityKey = _ObjContext.CreateEntityKey(_ObjSet.EntitySet.Name, entity);

            Object foundEntity;
            var exists = _ObjContext.TryGetObjectByKey(entityKey, out foundEntity);
            // TryGetObjectByKey attaches a found entity
            // Detach it here to prevent side-effects
            if (exists)
            {
                _ObjContext.Detach(foundEntity);
            }
            return (exists);
        }

        public T Entity(DbContext dbContext, Expression<Func<T, bool>> where)
        {
            return dbContext.Set<T>().AsNoTracking().Where(where).SingleOrDefault(); //去除缓存
        }

        public List<T> Entities(DbContext dbContext, Expression<Func<T, bool>> where)
        {
            return dbContext.Set<T>().AsNoTracking().Where<T>(where).AsQueryable().ToList();
        }

        public List<T> Entities(DbContext dbContext, Expression<Func<T, bool>> where, OrderModelField[] orderByExpression)
        {
            var query = dbContext.Set<T>().Where<T>(where);
            //创建表达式变量参数
            var parameter = Expression.Parameter(typeof(T), "o");
            if (orderByExpression != null && orderByExpression.Length > 0)
            {
                for (int i = 0; i < orderByExpression.Length; i++)
                {
                    // 根据属性名获取属性
                    var property = typeof(T).GetProperty(orderByExpression[i].PropertyName);
                    //创建一个访问属性的表达式
                    var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                    var orderByExp = Expression.Lambda(propertyAccess, parameter);
                    string OrderName = orderByExpression[i].IsDESC ? "OrderByDescending" : "OrderBy";
                    MethodCallExpression resultExp = Expression.Call(typeof(Queryable), OrderName, new Type[] { typeof(T), property.PropertyType }, query.Expression, Expression.Quote(orderByExp));
                    query = query.Provider.CreateQuery<T>(resultExp);
                }
            }

            return query.ToList();
        }

        public Page<T> FindPage(DbContext dbContext, Expression<Func<T, bool>> where, OrderModelField[] orderByExpression, int pageIndex, int pageSize)
        {
            if (pageIndex == 0) pageIndex = 1;
            if (pageSize == 0) pageSize = 20;

            var query = dbContext.Set<T>().Where(where).AsNoTracking();
            //创建表达式变量参数
            var parameter = Expression.Parameter(typeof(T), "o");
            if (orderByExpression != null && orderByExpression.Length > 0)
            {
                for (int i = 0; i < orderByExpression.Length; i++)
                {
                    // 根据属性名获取属性
                    var property = typeof(T).GetProperty(orderByExpression[i].PropertyName);
                    //创建一个访问属性的表达式
                    var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                    var orderByExp = Expression.Lambda(propertyAccess, parameter);
                    string OrderName = orderByExpression[i].IsDESC ? "OrderByDescending" : "OrderBy";
                    MethodCallExpression resultExp = Expression.Call(typeof(Queryable), OrderName, new Type[] { typeof(T), property.PropertyType }, query.Expression, Expression.Quote(orderByExp));
                    query = query.Provider.CreateQuery<T>(resultExp);
                }
            }
            var query1 = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            Page<T> p = new Page<T>
            {
                TotalRecord = query.Count(),
                ResultList = query1.ToList(),
                PageIndex = pageIndex,
                PageSize = pageSize,
                PageCount = (query.Count() + pageSize - 1) / pageSize
            };

            return p;
        }

        public int ExecuteSqlCommand(DbContext dbContext, string sql, params object[] parameters)
        {
            return dbContext.Database.ExecuteSqlCommand(sql, parameters);
        }

        public List<T> Entities(DbContext dbContext, string sql, params object[] parameters)
        {
            return dbContext.Database.SqlQuery<T>(sql, parameters).ToList();
        }

        public Page<K> FindPage<K>(DbContext dbContext, Pageination page)
        {
            if (page.PageIndex == 0)
                page.PageIndex = 1;
            if (page.PageSize == 0)
                page.PageIndex = 10;

            SqlParameter[] paras = new SqlParameter[11];
            paras[0] = new SqlParameter("strTable", DbType.String);
            paras[0].Value = page.StrTable;

            paras[1] = new SqlParameter("strField", DbType.String);
            paras[1].Value = page.StrField;

            paras[2] = new SqlParameter("pageSize", DbType.Int16);
            paras[2].Value = page.PageSize;

            paras[3] = new SqlParameter("pageIndex", DbType.Int16);
            paras[3].Value = page.PageIndex;

            paras[4] = new SqlParameter("strWhere", DbType.String);
            paras[4].Value = page.StrWhere;

            paras[5] = new SqlParameter("strSortKey", DbType.String);
            paras[5].Value = page.StrSortKey;

            paras[6] = new SqlParameter("strSortField", DbType.String);
            paras[6].Value = page.StrSortField;

            paras[7] = new SqlParameter("strOrderBy", DbType.Boolean);
            paras[7].Value = page.StrOrderBy;

            paras[8] = new SqlParameter("strGroupField", DbType.String);
            paras[8].Value = page.StrGroupField;

            paras[9] = new SqlParameter("RecordCount", DbType.Int16);
            paras[9].Value = page.RecordCount;
            paras[9].Direction = ParameterDirection.Output;

            paras[10] = new SqlParameter("UsedTime", DbType.Int16);
            paras[10].Value = page.UsedTime;
            paras[10].Direction = ParameterDirection.Output;

            string sql = @"exec Pager @strTable,@strField,@pageSize,@pageIndex,@strWhere,@strSortKey,@strSortField,@strOrderBy,@strGroupField,@RecordCount output,@UsedTime output";
            List<K> result = dbContext.Database.SqlQuery<K>(sql, paras).ToList();

            Page<K> p = new Page<K>
            {
                TotalRecord = (int)paras[9].Value,
                UsedTime = (int)paras[10].Value,
                ResultList = result,
                page = page.PageIndex,
                rows = page.PageSize
            };
            p.PageCount = (p.TotalRecord + p.rows - 1) / p.rows;

            return p;
        }
    }
}
