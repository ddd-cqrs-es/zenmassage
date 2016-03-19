using AggregateSource;
using Zen.Massage.Domain.GeneralBoundedContext;

namespace Zen.Massage.Domain.UserBoundedContext
{
    public class TherapistSkillEntityState : EntityState
    {
        public ITherapist Therapist { get; set; }

        public TherapyId TherapyId { get; private set; }

        public TherapistSkillVerificationState VerificationStatus { get; private set; }
    }
}