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

        protected virtual string GetStringKeyPartition(TKey key)
        {
            return key.GetType().Name.Replace("Aggregate", string.Empty);
        }

        protected virtual string GetStringKeyRaw(TKey key)
        {
            return key.ToString();
        }

        protected virtual string GetStringKeyFromBlob(TKey key)
        {
            var partition = GetStringKeyPartition(key);
            var identifier = GetStringKeyRaw(key);

            if (!string.IsNullOrEmpty(partition))
            {
                return $"{partition}#{identifier}";
            }
            else
            {
                return identifier;
            }
        }
    }
}
