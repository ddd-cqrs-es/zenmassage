// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommitableUnitOfWork.cs" company="Zen Design Software">
//   © Zen Design Software 2015
// </copyright>
// <summary>
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Transactions;
using AggregateSource;
using NEventStore;

namespace Zen.Infrastructure.WriteRepository
{
    /// <summary>
    /// The commitable unit of work.
    /// </summary>
    public class CommitableUnitOfWork : UnitOfWork
    {
        /// <summary>
        /// The _store events.
        /// </summary>
        private readonly IStoreEvents _storeEvents;

        /// <summary>
        /// Initialises a new instance of the <see cref="CommitableUnitOfWork" />
        /// class.
        /// </summary>
        /// <param name="storeEvents">
        /// The store events.
        /// </param>
        public CommitableUnitOfWork(IStoreEvents storeEvents)
        {
            _storeEvents = storeEvents;
        }

        /// <summary>
        /// Gets a value indicating whether use transaction scope.
        /// </summary>
        public bool UseTransactionScope
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// The commit.
        /// </summary>
        public void Commit()
        {
            if (HasChanges())
            {
                // Walk the list of aggregates in the change-set
                var firstAggregate = true;
                foreach (var aggregate in GetChanges())
                {
                    // We should only ever update a single aggregate per transaction
                    // 	so issue warning if this isn't the first aggregate
                    if (!firstAggregate)
                    {
                        // TODO: Log to logger
                    }

                    // Create commit id so we can compensate if necessary
                    var commitId = Guid.NewGuid();

                    // Commit the aggregate
                    if (UseTransactionScope)
                    {
                        // Wrap commit with a transaction scope
                        using (var scope = new TransactionScope())
                        {
                            CommitAggregate(aggregate, commitId);

                            // Commit the transaction
                            scope.Complete();
                        }
                    }
                    else
                    {
                        CommitAggregate(aggregate, commitId);
                    }

                    // At this point we can clear the changes from the aggregate
                    aggregate.Root.ClearChanges();

                    // No longer processing first aggregate...
                    firstAggregate = false;
                }
            }
        }

        /// <summary>
        /// The commit aggregate.
        /// </summary>
        /// <param name="aggregate">
        /// The aggregate.
        /// </param>
        /// <param name="commitId">
        /// The commit id.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        /// <exception cref="ConcurrencyException">
        /// </exception>
        private void CommitAggregate(Aggregate aggregate, Guid commitId)
        {
            // Aggregate identifier has bucket id concatenated with stream id
            var parts = aggregate.Identifier.Split('#');
            if (parts.Length > 2)
            {
                throw new InvalidOperationException();
            }

            IEventStream stream = null;
            try
            {
                // Open or create the stream for this aggregate
                if (parts.Length == 2)
                {
                    stream = _storeEvents.OpenStream(parts[0], parts[1], 0, int.MaxValue);
                }
                else
                {
                    stream = _storeEvents.OpenStream(parts[0], 0);
                }

                // Concurrency check
                if (stream.StreamRevision != aggregate.ExpectedVersion)
                {
                    throw new ConcurrencyException();
                }

                // Write each event to the underlying stream
                foreach (var eventObject in aggregate.Root.GetChanges())
                {
                    // Create new message
                    var message = new EventMessage { Body = eventObject };

                    /*// If the aggregate is derived from ITenantAggregate
                    // 	then store TenantId in the message headers
                    // 	as this will be used later to determine the servicebus
                    // 	partition to use for enterprise-level routing
                    var tenantAggregate = aggregate.Root as ITenantAggregate;
                    if (tenantAggregate != null)
                    {
                        message.Headers.Add("TenantId", tenantAggregate.TenantId);
                    }*/

                    // Add message to the event store
                    stream.Add(message);
                }

                // Commit the stream
                stream.CommitChanges(commitId);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Dispose();
                    stream = null;
                }
            }
        }
    }
}