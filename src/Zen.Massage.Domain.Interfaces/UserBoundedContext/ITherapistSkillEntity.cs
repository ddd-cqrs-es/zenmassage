using System;
using Zen.Massage.Domain.GeneralBoundedContext;

namespace Zen.Massage.Domain.UserBoundedContext
{
    public interface ITherapistSkillEntity
    {
        TherapyId TherapyId { get; }

        TherapistSkillVerificationState VerificationStatus { get; }

        DateTimeOffset AcquisitionDate { get; }

        DateTimeOffset ApplicationDate { get; }

        DateTimeOffset? VerificationDate { get; }

        DateTimeOffset? ExpiryDate { get; }
    }
}