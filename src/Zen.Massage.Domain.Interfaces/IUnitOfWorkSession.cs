using System;

namespace Zen.Massage.Domain
{
    public interface IUnitOfWorkSession : IDisposable
    {
        TRepository GetRepository<TRepository>();

        void Commit();
    }
}