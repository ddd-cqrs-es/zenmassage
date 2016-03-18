using AggregateSource;

namespace Zen.Massage.Domain.UserBoundedContext
{
    public class TherapistSkillEntityState : EntityState
    {
        public ITherapist Therapist { get; set; }

        public TherapistSkillVerificationState VerificationStatus { get; private set; }
    }
}