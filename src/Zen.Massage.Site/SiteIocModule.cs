using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using NanoMessageBus;
using NanoMessageBus.Channels;
using NEventStore;
using Zen.Infrastructure.ReadRepository;
using Zen.Massage.Application;
using Zen.Massage.Domain.BookingContext;
using Module = Autofac.Module;

namespace Zen.Massage.Site
{
    public class SiteIocModule : Module
    {
        // TODO: Pass ASP.NET configuration object into module
        //  so setup parameters can be setup via IoC

        protected override void Load(ContainerBuilder builder)
        {
            // Setup messagebus (we're using lightweight message bus rather than Azure ServiceBus)
            var routingTable =
                new AutofacRoutingTable(
                    builder,
                    Assembly.GetExecutingAssembly(),
                    typeof(BookingUpdater).Assembly);
            var messagingHost = new MessagingWireup()
                //.WithAuditing()   TODO: Hookup auditor that is capable of writing aggregate information to AppInsights
                .StartWithReceive(routingTable);
            
            // Setup event store
            builder.Register(c => BuildEventStore(c.Resolve<ILifetimeScope>()))
                .As<IStoreEvents>()
                .SingleInstance();

            // Register core domain types
            builder.RegisterType<BookingFactory>()
                .As<IBookingFactory>();

            // Register infrastructure types
            builder.RegisterType<BookingReadRepository>()
                .As<IBookingReadRepository>();
            builder.RegisterType<IBookingWriteRepository>()
                .As<IBookingWriteRepository>();

            // Register application types

            // Register site types
        }

        private IStoreEvents BuildEventStore(ILifetimeScope container)
        {
            return Wireup.Init()
                .UsingSqlPersistence("EventStore")
                    .InitializeStorageEngine()
                .UsingJsonSerialization()
                .Compress()
                .HookIntoPipelineUsing(new PipelineDispatcherHook(container))
                .Build();
        }

        public class PipelineDispatcherHook : IPipelineHook
        {
            private readonly ILifetimeScope _container;

            public PipelineDispatcherHook(ILifetimeScope container)
            {
                _container = container;
            }

            public void Dispose()
            {
            }

            public ICommit Select(ICommit committed)
            {
                return committed;
            }

            public bool PreCommit(CommitAttempt attempt)
            {
                return true;
            }

            public void PostCommit(ICommit committed)
            {
                using (var scope = _container.BeginLifetimeScope())
                {
                    // Get dispatch context
                    var publisher = scope.Resolve<IDispatchContext>();

                    // Publish events and commit
                    publisher
                        .Publish(committed.Events.Select(e => e.Body).ToArray())
                        .Commit();
                }
            }

            public void OnPurge(string bucketId)
            {
            }

            public void OnDeleteStream(string bucketId, string streamId)
            {
            }
        }
    }
}
