namespace Zen.Massage.Domain
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWorkSession CreateSession();
    }
}