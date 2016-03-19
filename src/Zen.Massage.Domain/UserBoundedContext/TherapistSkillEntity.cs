using System;
using AggregateSource;
using Zen.Massage.Domain.GeneralBoundedContext;

namespace Zen.Massage.Domain.UserBoundedContext
{
    public class TherapistSkillEntity : Entity<TherapistSkillEntityState>, ITherapistSkillEntity
    {
        public TherapistSkillEntity(ITherapist owner, Action<object> applier)
            : base(applier)
        {
            State.Therapist = owner;
        }

        public TherapyId TherapyId => State.TherapyId;

        public TherapistSkillVerificationState VerificationStatus => State.VerificationStatus;
        public DateTimeOffset AcquisitionDate { get; }
        public DateTimeOffset ApplicationDate { get; }
        public DateTimeOffset? VerificationDate { get; }
        public DateTimeOffset? ExpiryDate { get; }
    }
}