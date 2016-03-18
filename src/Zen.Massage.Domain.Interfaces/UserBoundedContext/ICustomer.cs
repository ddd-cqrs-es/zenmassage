namespace Zen.Massage.Domain.UserBoundedContext
{
    public interface ICustomer
    {
        CustomerId CustomerId { get; }

        IClientBasicInformation BasicInformation { get; }
    }
}