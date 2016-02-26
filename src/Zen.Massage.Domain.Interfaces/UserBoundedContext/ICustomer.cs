namespace Zen.Massage.Domain.UserBoundedContext
{
    public interface ICustomer : IBaseClient
    {
        CustomerId CustomerId { get; }
    }
}