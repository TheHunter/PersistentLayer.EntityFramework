using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PersistentLayer.EntityFramework.Impl
{
    public class EfRootPagedDAO<TRootEntity>
        : EfContextProviderDAO, IRootPagedDAO<TRootEntity>
        where TRootEntity : class
    {
        public EfRootPagedDAO(IEfTransactionProvider transactionProvider)
            : base(transactionProvider)
        {
        }

        public TEntity FindBy<TEntity>(object identifier) where TEntity : class, TRootEntity
        {
            return this.TransactionProvider
                .ContextProvider
                .FindBy<TEntity>(identifier);
        }

        public bool Exists<TEntity>(object identifier) where TEntity : class, TRootEntity
        {
            return this.TransactionProvider
                .ContextProvider
                .Exists<TEntity>(identifier);
        }

        public bool Exists<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, TRootEntity
        {
            return this.TransactionProvider
                .ContextProvider
                .Exists(predicate);
        }
        public bool Exists<TEntity>(ICollection identifiers) where TEntity : class, TRootEntity
        {
            return identifiers.Cast<object>()
                .All(this.Exists<TEntity>);
        }

        public TEntity UniqueResult<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, TRootEntity
        {
            return this.TransactionProvider
                .ContextProvider
                .UniqueResult(predicate);
        }

        public IEnumerable<TEntity> FindAll<TEntity>() where TEntity : class, TRootEntity
        {
            return this.TransactionProvider
                .ContextProvider
                .FindAll<TEntity>();
        }

        public IEnumerable<TEntity> FindAll<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, TRootEntity
        {
            return this.TransactionProvider
                .ContextProvider
                .FindAll(predicate);
        }

        public TResult ExecuteExpression<TEntity, TResult>(Expression<Func<IQueryable<TEntity>, TResult>> queryExpr) where TEntity : class, TRootEntity
        {
            return this.TransactionProvider
                .ContextProvider
                .ExecuteExpression(queryExpr);
        }
        
        public IPagedResult<TEntity> GetPagedResult<TEntity>(int startIndex, int pageSize, Expression<Func<TEntity, bool>> predicate) where TEntity : class, TRootEntity
        {
            return this.TransactionProvider
                .ContextProvider
                .GetPagedResult(startIndex, pageSize, predicate);
        }

        public ITransactionProvider GetTransactionProvider()
        {
            return this.TransactionProvider;
        }

        public TEntity MakePersistent<TEntity>(TEntity entity) where TEntity : class, TRootEntity
        {
            //var aaa = base.CurrentContext.Entry(entity);
            //base.ObjectContext.CreateEntityKey()
            
            //base.ObjectContext.TryGetObjectByKey()
            throw new NotImplementedException();
        }

        public TEntity MakePersistent<TEntity>(TEntity entity, object identifier) where TEntity : class, TRootEntity
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> MakePersistent<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, TRootEntity
        {
            throw new NotImplementedException();
        }

        public void MakeTransient<TEntity>(TEntity entity) where TEntity : class, TRootEntity
        {
            throw new NotImplementedException();
        }

        public void MakeTransient<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, TRootEntity
        {
            throw new NotImplementedException();
        }
    }


    public class EfRootPagedDAO<TRootEntity, TEntity>
        : EfContextProviderDAO, IRootPagedDAO<TRootEntity, TEntity>
        where TRootEntity : class
        where TEntity : class, TRootEntity
    {
        public EfRootPagedDAO(IEfTransactionProvider transactionProvider)
            : base(transactionProvider)
        {
        }
        public TEntity FindBy(object identifier)
        {
            return this.TransactionProvider
                .ContextProvider
                .FindBy<TEntity>(identifier);
        }

        public bool Exists(object identifier)
        {
            return this.TransactionProvider
                .ContextProvider
                .Exists<TEntity>(identifier);
        }

        public bool Exists(Expression<Func<TEntity, bool>> predicate)
        {
            return this.TransactionProvider
                .ContextProvider
                .Exists(predicate);
        }

        public bool Exists(ICollection identifiers)
        {
            return identifiers.Cast<object>()
                .All(this.Exists);
        }

        public TEntity UniqueResult(Expression<Func<TEntity, bool>> predicate)
        {
            return this.TransactionProvider
                .ContextProvider
                .UniqueResult(predicate);
        }

        public IEnumerable<TEntity> FindAll()
        {
            return this.TransactionProvider
                .ContextProvider
                .FindAll<TEntity>();
        }

        public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate)
        {
            return this.TransactionProvider
                .ContextProvider
                .FindAll(predicate);
        }

        public TResult ExecuteExpression<TResult>(Expression<Func<IQueryable<TEntity>, TResult>> queryExpr)
        {
            return this.TransactionProvider
                .ContextProvider
                .ExecuteExpression(queryExpr);
        }

        public IPagedResult<TEntity> GetPagedResult(int startIndex, int pageSize, Expression<Func<TEntity, bool>> predicate)
        {
            return this.TransactionProvider
                .ContextProvider
                .GetPagedResult(startIndex, pageSize, predicate);
        }

        public ITransactionProvider GetTransactionProvider()
        {
            return this.TransactionProvider;
        }

        public TEntity MakePersistent(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public TEntity MakePersistent(TEntity entity, object identifier)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> MakePersistent(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public void MakeTransient(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void MakeTransient(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }
    }
}
