namespace Zen.Massage.Domain.UserBoundedContext
{
    public interface IBaseClient
    {
        IClientBasicInformation BasicInformation { get; }
    }
}