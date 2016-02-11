// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitOfWorkFactory.cs" company="Zen Design Software">
//   © Zen Design Software 2015
// </copyright>
// <summary>
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Transactions;
using Autofac;
using Zen.Massage.Domain;

namespace Zen.Infrastructure.WriteRepository
{
    /// <summary>
    /// The unit of work factory.
    /// </summary>
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        /// <summary>
        /// The _container.
        /// </summary>
        private readonly IComponentContext _container;

        /// <summary>
        /// Initialises a new instance of the <see cref="UnitOfWorkFactory" /> class.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        public UnitOfWorkFactory(IComponentContext container)
        {
            _container = container;
        }

        /// <summary>
        /// The create session.
        /// </summary>
        /// <returns>
        /// The <see cref="IUnitOfWorkSession" />.
        /// </returns>
        public IUnitOfWorkSession CreateSession()
        {
            return
                _container.Resolve<UnitOfWorkSession>(
                    new NamedParameter("scopeOption", TransactionScopeOption.Required),
                    new NamedParameter("scopeTimeout", TimeSpan.FromSeconds(30)));
        }
    }
}