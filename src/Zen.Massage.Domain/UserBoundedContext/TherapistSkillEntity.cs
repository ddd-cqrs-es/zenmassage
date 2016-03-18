using System;
using AggregateSource;

namespace Zen.Massage.Domain.UserBoundedContext
{
    public class TherapistSkillEntity : Entity<TherapistSkillEntityState>, ITherapistSkillEntity
    {
        public TherapistSkillEntity(ITherapist owner, Action<object> applier)
            : base(applier)
        {
            State.Therapist = owner;
        }

        public TherapistSkillVerificationState VerificationStatus => State.VerificationStatus;
    }
}