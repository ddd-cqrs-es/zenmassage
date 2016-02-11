// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitOfWorkSession.cs" company="Zen Design Software">
//   © Zen Design Software 2015
// </copyright>
// <summary>
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Transactions;
using Autofac;
using NEventStore;
using Zen.Massage.Domain;

namespace Zen.Infrastructure.WriteRepository
{
    /// <summary>
    /// The unit of work session.
    /// </summary>
    public class UnitOfWorkSession : IUnitOfWorkSession
    {
        /// <summary>
        /// The _container.
        /// </summary>
        private readonly IComponentContext _container;

        /// <summary>
        /// The _repositories.
        /// </summary>
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        /// <summary>
        /// The _unit of work.
        /// </summary>
        private CommitableUnitOfWork _unitOfWork;

        /// <summary>
        /// Initialises a new instance of the <see cref="UnitOfWorkSession" /> class.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        /// <param name="scopeOption">
        /// The scope option.
        /// </param>
        /// <param name="scopeTimeout">
        /// The scope timeout.
        /// </param>
        public UnitOfWorkSession(IComponentContext container, TransactionScopeOption scopeOption, TimeSpan scopeTimeout)
        {
            _container = container;
            _unitOfWork = new CommitableUnitOfWork(container.Resolve<IStoreEvents>());
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            // We don't actually have anything to do here since a new session
            // 	will use a new UOW and force loading of new aggregates.
            _unitOfWork = null;
        }

        /// <summary>
        /// The get repository.
        /// </summary>
        /// <typeparam name="TRepository">
        /// </typeparam>
        /// <returns>
        /// The <see cref="TRepository" />.
        /// </returns>
        public TRepository GetRepository<TRepository>()
        {
            var repositoryType = typeof(TRepository);
            if (!_repositories.ContainsKey(repositoryType))
            {
                _repositories.Add(
                    repositoryType,
                    _container.Resolve<TRepository>(new NamedParameter("unitOfWork", _unitOfWork)));
            }

            return (TRepository)_repositories[repositoryType];
        }

        /// <summary>
        /// The commit.
        /// </summary>
        /// <returns>
        /// The <see cref="ObjectDisposedException" />.
        /// </returns>
        /// <exception cref="Task">
        /// </exception>
        public void Commit()
        {
            if (_unitOfWork == null)
            {
                throw new ObjectDisposedException(GetType().FullName, "Session has already been disposed.");
            }

            _unitOfWork.Commit();
        }
    }
}