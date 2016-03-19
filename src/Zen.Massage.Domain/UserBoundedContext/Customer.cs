namespace Zen.Massage.Domain.UserBoundedContext
{
    public class Customer : ICustomer
    {
        public CustomerId CustomerId { get; private set; }

        public IClientBasicInformation BasicInformation { get; private set; }
    }
}
