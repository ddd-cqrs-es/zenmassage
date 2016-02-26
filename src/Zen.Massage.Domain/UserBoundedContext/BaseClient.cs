namespace Zen.Massage.Domain.UserBoundedContext
{
    public class BaseClient : IBaseClient
    {
        public IClientBasicInformation BasicInformation { get; private set; }      
    }
}