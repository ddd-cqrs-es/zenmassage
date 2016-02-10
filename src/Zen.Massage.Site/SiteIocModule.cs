using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using NanoMessageBus;
using NEventStore;
using Zen.Infrastructure.ReadRepository;
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
            var messagingHost = new MessagingWireup()
                .WithAuditing()
                .StartWithReceive(new AutofacRoutingTable(builder, Assembly.GetExecutingAssembly()));

            // Setup event store
            var store = Wireup.Init()
                .UsingInMemoryPersistence()     // TODO: Switch to SQL connection for persistence
                .UsingJsonSerialization()
                .Compress()
                //.EncryptWith()                // TODO: Enable encryption (on a per-tenant basis eventually)
                .HookIntoPipelineUsing()        // TODO: Hook into processing pipeline so we can route events
                                                //  through the application message bus queue
                .Build();
            builder.RegisterInstance(store);

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
    }
}
