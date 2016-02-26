namespace Zen.Massage.Domain.UserBoundedContext
{
    public class Therapist : BaseClient, ITherapist
    {
        public TherapistId TherapistId { get; private set; }
    }
}