namespace Zen.Massage.Domain.UserBoundedContext
{
    public interface ITherapist : IBaseClient
    {
        TherapistId TherapistId { get; }
    }
}