using System;

namespace Zen.Massage.Domain.UserBoundedContext
{
    public interface ITherapistSkillEntity
    {
        TherapyId TherapyId { get; }

        TherapistSkillVerificationState VerificationStatus { get; }

        DateTimeOffset DateAcquired { get; }

        DateTimeOffset DateApplied { get; }

        DateTimeOffset? DateVerified { get; }

        DateTimeOffset? DateExpires { get; }
    }
}