namespace Zen.Massage.Domain.UserBoundedContext
{
    public interface ITherapistFactory
    {
        ITherapist Create(TherapistId therapistId);
    }
}