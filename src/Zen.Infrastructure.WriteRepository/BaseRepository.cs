using AggregateSource;
using AggregateSource.NEventStore;
using NEventStore;

namespace Zen.Infrastructure.WriteRepository
{
    public abstract class BaseRepository<TAggregate, TKey> : Repository<TAggregate>
        where TAggregate : class, IAggregateRootEntity, new()
    {
        protected BaseRepository(UnitOfWork unitOfWork, IStoreEvents eventStore)
            : base(() => new TAggregate(), unitOfWork, eventStore)
        {
        }

        public void AddAggregate(TAggregate value)
        {
            var key = GetKeyFromAggregate(value);
            var stringKey = GetStringKeyFromBlob(key);
            Add(stringKey, value);
        }

        protected TAggregate GetAggregate(TKey key)
        {
            var stringKey = GetStringKeyFromBlob(key);
            return Get(stringKey);
        }

        protected TAggregate GetAggregateOptional(TKey key)
        {
            var stringKey = GetStringKeyFromBlob(key);
            var result = GetOptional(stringKey);
            if (result.HasValue)
            {
                return result.Value;
            }
            return null;
        }

        protected abstract TKey GetKeyFromAggregate(TAggregate aggregate);

        protected virtual string GetStringKeyFromBlob(TKey key)
        {
            return key.ToString();
        }
    }
}
