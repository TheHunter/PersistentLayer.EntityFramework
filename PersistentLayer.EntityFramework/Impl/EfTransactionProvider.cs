using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using PersistentLayer.Exceptions;
using PersistentLayer.Impl;

namespace PersistentLayer.EntityFramework.Impl
{
    public class EfTransactionProvider
        : IEfTransactionProvider
    {
        private readonly List<ITransactionWorker> transactions;

        public EfTransactionProvider(IContextProvider contextProvider)
        {
            this.ContextProvider = contextProvider;
            this.transactions = new List<ITransactionWorker>();
        }

        public IContextProvider ContextProvider { get; }

        public bool InProgress
        {
            get { return this.transactions.Count > 0; }
        }

        public bool Exists(string name)
        {
            if (name == null)
                return false;

            return this.transactions.Any(info => info.Name.Equals(name));
        }

        public void BeginTransaction()
        {
            this.BeginTransaction((IsolationLevel?)null);
        }

        public void BeginTransaction(string name)
        {
            this.BeginTransaction(name, (IsolationLevel?)null);
        }

        public void BeginTransaction(IsolationLevel? level)
        {
            this.BeginTransaction(Guid.NewGuid().ToString("D"), level);
        }

        public void BeginTransaction(string name, IsolationLevel? level)
        {
            if (name == null || name.Trim().Equals(string.Empty))
                throw new BusinessLayerException("The transaction name cannot be null or empty", "BeginTransaction");

            this.Begin(new TransactionDescriptor { Name = new Guid().ToString("D"), Isolation = level });
        }

        public ITransactionWorker Begin(TransactionDescriptor descriptor = null)
        {
            descriptor = descriptor ?? new TransactionDescriptor { Name = new Guid().ToString("D") };

            if (this.Exists(descriptor.Name))
                throw new BusinessLayerException(string.Format("The transaction name ({0}) to add is used by another point.", descriptor.Name), "BeginTransaction");

            ITransactionWorker info;
            if (this.transactions.Count == 0)
            {
                try
                {
                    var context = this.ContextProvider.GetCurrentContext();
                    var transaction = descriptor.Isolation == null ? context.Database.BeginTransaction() : context.Database.BeginTransaction(descriptor.Isolation.Value);
                    info = new TransactionWorkerImpl(descriptor.Name,
                        desc => { },
                        desc =>
                        {
                            var tran = this.RemoveTransaction(desc.Name);
                            try
                            {
                                context.SaveChanges();
                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                throw new CommitFailedException(string.Format("Error when the current session tries to commit the current transaction (name: {0}).", tran.Name), "CommitTransaction", ex);
                            }
                        },
                        desc =>
                        {
                            var tran = this.RemoveTransaction(desc.Name);
                            try
                            {
                                transaction.Rollback();
                            }
                            catch (Exception cause)
                            {
                                throw new RollbackFailedException("Error on calling RollbackTransaction method", cause, tran);
                            }
                        }
                        );
                }
                catch (Exception ex)
                {
                    throw new BusinessLayerException("Error on beginning a new transaction.", "BeginTransaction", ex);
                }
            }
            else
            {
                info = new TransactionWorkerImpl(descriptor.Name,
                    desc => { },
                    desc =>
                    {
                        this.RemoveTransaction(desc.Name);
                    },
                    desc =>
                    {
                        var tran = this.RemoveTransaction(desc.Name);
                        throw new InnerRollBackException("An inner rollback transaction has occurred.", tran);
                    }
                    );
            }
            this.transactions.Add(info);

            return info;
        }

        public void CommitTransaction()
        {
            ITransactionWorker info = this.PopTransaction();
            if (info != null)
            {
                info.Commit();
            }
        }

        public void RollbackTransaction()
        {
            this.RollbackTransaction(null);
        }

        public void RollbackTransaction(Exception cause)
        {
            ITransactionWorker info = this.PopTransaction();
            if (info != null)
            {
                info.Rollback();
            }
        }

        public void Dispose()
        {
            while (this.InProgress)
            {
                var tran = this.PopTransaction();
                try
                {
                    tran.Rollback();
                }
                catch (Exception)
                {
                    this.transactions.Clear();
                }
            }
        }

        private ITransactionWorker RemoveTransaction(string name)
        {
            var transaction =
                this.transactions.Find(worker => worker.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

            this.transactions.Remove(transaction);

            return transaction;
        }

        private ITransactionWorker PopTransaction()
        {
            if (transactions.Count == 0)
                return null;

            int lastIndex = transactions.Count - 1;
            ITransactionWorker info = transactions[lastIndex];
            this.transactions.RemoveAt(lastIndex);

            return info;
        }
    }
}
