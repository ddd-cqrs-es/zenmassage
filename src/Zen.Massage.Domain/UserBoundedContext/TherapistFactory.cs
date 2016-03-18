namespace Zen.Massage.Domain.UserBoundedContext
{
    public class TherapistFactory
    {
        public ITherapist Create(TherapistId therapistId)
        {
            return new TherapistAggregate();
        }
    }
}