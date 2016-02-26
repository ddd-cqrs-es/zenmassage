namespace Zen.Massage.Domain.UserBoundedContext
{
    public class Customer : BaseClient, ICustomer
    {
        public CustomerId CustomerId { get; private set; }
    }
}
