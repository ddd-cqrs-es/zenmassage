namespace Zen.Massage.Domain.UserBoundedContext
{
    public class TherapistFactory : ITherapistFactory
    {
        public ITherapist Create(TherapistId therapistId)
        {
            var therapist = new TherapistAggregate();
            therapist.Create(therapistId);
            return therapist;
        }
    }
}