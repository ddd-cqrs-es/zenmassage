using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using NEventStore;
using Zen.Infrastructure.ReadRepository;
using Zen.Massage.Domain.BookingContext;

namespace Zen.Massage.Site
{
    public class SiteIocModule : Module
    {
        // TODO: Pass ASP.NET configuration object into module
        //  so setup parameters can be setup via IoC

        protected override void Load(ContainerBuilder builder)
        {
            // Setup event store
            var store = Wireup.Init()
                .UsingInMemoryPersistence() // TODO: Switch to Azure document DB persistence scheme
                .UsingJsonSerialization()
                .Compress()
                //.EncryptWith()                // TODO: Enable encryption (on a per-tenant basis eventually)
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
